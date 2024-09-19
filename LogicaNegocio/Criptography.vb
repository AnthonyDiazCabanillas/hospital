Imports System.Security.Cryptography
Imports System.Text

Public Class Criptography

    Private Const CharactersKey As String = "rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5" 'No se puede alterar la cantidad de caracteres.
    Private Const EncryptKey As String = "csf3lipe"
    Private Const ApiKey As String = "CL&NIC@$@NF3L1P32022..#$"
    Public Function EncryptConectionString(pConectionStringDecrypt As String) As String
        Dim IV As Byte() = ASCIIEncoding.ASCII.GetBytes(EncryptKey)
        Dim EncryptionKey As Byte() = Convert.FromBase64String(CharactersKey)
        Dim buffer As Byte() = Encoding.UTF8.GetBytes(pConectionStringDecrypt)
        Dim des As TripleDES = TripleDES.Create()
        des.Key = EncryptionKey
        des.IV = IV
        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
    End Function

    Public Property _ApiCLinica() As String
        Get
            Return ApiKey
        End Get
        Set(value As String)
        End Set
    End Property


End Class


'Byte[] IV = ASCIIEncoding.ASCII.GetBytes(EncryptKey);
'            Byte[] EncryptionKey = Convert.FromBase64String(CharactersKey);
'            Byte[] buffer = Encoding.UTF8.GetBytes(pConectionStringDecrypt);
'            //TripleDESCryptoServiceProvider des = New TripleDESCryptoServiceProvider();            
'            TripleDES des = TripleDES.Create();
'            des.Key = EncryptionKey;
'            des.IV = IV;
'
'            Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));