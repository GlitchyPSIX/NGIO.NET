using System;
using System.IO;
using System.Security.Cryptography;
using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Converters {
    /// <summary>
    /// This Converter encrypts the Execute components of the request if Secure is enabled in the request. Does not read JSON.
    /// </summary>
    public class NgioSecureRequestConverter : JsonConverter<NgioServerRequest> {
        public override bool CanRead => false; // This converter is intended to change the Execution Request object into a Secure Execution request
        public byte[] EncryptionKey { get; set; }

        public override void WriteJson(JsonWriter writer, NgioServerRequest value, JsonSerializer serializer) {
            JObject newObj = new JObject(
                new JProperty("app_id", value.AppId),
                new JProperty("debug", value.Debug)
            );
            if (value.SessionId != null) newObj.Add(new JProperty("session_id", value.SessionId));
            if (value.Echo != null) newObj.Add(new JProperty("echo", value.Echo));

            if (value.ExecutedComponents.Length == 0)
                throw new ArgumentException("NGIO.NET Error: No components have been sent!");

            JArray executeComponents = JArray.FromObject(value.ExecutedComponents, serializer);

            if (value.SecurityLevel == SecurityLevel.None) { // If the security level is none, write as-is
                foreach (JToken executeComponent in executeComponents) {
                    if (executeComponent is JObject jOExecuteComponent) { // safety net just in case, but they should all be JObjects
                        jOExecuteComponent.Remove("_ngionet_requiresSecure"); // strip away ngio.net specific secure flag from all components
                    }
                }

                newObj.Add(new JProperty("execute", executeComponents));
            }
            else { // Encrypt with AES-128, append IV at the beginning, encode to base64
                // TODO: Each component is one individual "secure" call inside the execute object.

                using (Aes aesEncrypt = Aes.Create()) {
                    aesEncrypt.BlockSize = 128;
                    aesEncrypt.GenerateIV();
                    aesEncrypt.Key = EncryptionKey;

                    ICryptoTransform aesEncryptTransform = aesEncrypt.CreateEncryptor(aesEncrypt.Key, aesEncrypt.IV);

                    JArray execObject = new JArray();

                    foreach (JToken executeComponent in executeComponents) {
                        bool requiresEncryption = executeComponent["_ngionet_requiresSecure"].ToObject<bool>();
                        if (executeComponent is JObject jOExecuteComponent) { // safety net just in case, but this should be a JObject here
                            jOExecuteComponent.Remove("_ngionet_requiresSecure"); // strip away ngio.net specific secure flag
                        }

                        // if it doesn't require encryption or security level is not forceAll
                        if (! ((requiresEncryption && value.SecurityLevel == SecurityLevel.OnlyRequired) ||
                            (value.SecurityLevel == SecurityLevel.ForceAll))) {
                            execObject.Add(executeComponent); // add as-is
                            continue;
                        }

                        byte[] encrypted;
                        string base64Encrypted;
                        // wrapping?
                        string jString = executeComponents.First.ToString();

                        using (MemoryStream msEncrypt = new MemoryStream()) {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesEncryptTransform, CryptoStreamMode.Write)) {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                                    swEncrypt.Write(jString);
                                }
                            }

                            byte[] memStrArr = msEncrypt.ToArray();

                            // Will store the IV + encrypted json
                            encrypted = new byte[aesEncrypt.IV.Length + msEncrypt.ToArray().Length];
                            Buffer.BlockCopy(aesEncrypt.IV, 0, encrypted, 0, aesEncrypt.IV.Length);
                            Buffer.BlockCopy(memStrArr, 0, encrypted, aesEncrypt.IV.Length, memStrArr.Length);
                            base64Encrypted = Convert.ToBase64String(encrypted);
                        }

                        JObject secureObject = new JObject(
                            new JProperty ("secure", base64Encrypted));
                        execObject.Add(secureObject); // add secure object to execution array

                    }
                    newObj.Add(new JProperty("execute", execObject)); // add execution array to request object
                }
            }
            newObj.WriteTo(writer);
            writer.Flush();
        }

        public override NgioServerRequest ReadJson(JsonReader reader, Type objectType, NgioServerRequest existingValue, bool hasExistingValue,
            JsonSerializer serializer) {
            return null;
        }
    }
}