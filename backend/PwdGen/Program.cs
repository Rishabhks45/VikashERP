using System;
using VikashERP.SharedKernel.Services;

var encryptionService = new EncryptionService();
var masterKey = "aU5FU1RIQY5NUzU3Q1JFVEtFWTk4NzY1NDMyMUFCQ0RFRkdISUdLTE1OTw==";

var targetHash = "RRCbZND+orT8Q/uQPUYrxoGF970IawlrzgS4MjWMPaKIGTcpBrN5CfgzhHI7NlHycw==";
try
{
    var decrypted = encryptionService.Decrypt(targetHash, masterKey);
    Console.WriteLine($"Decrypted Stored Password: '{decrypted}'");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to decrypt: {ex.Message}");
}
