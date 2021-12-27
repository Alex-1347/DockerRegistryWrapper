Imports Docker.Registry.DotNet
Imports Docker.Registry.DotNet.Authentication

Module Program
    Sub Main(args As String())
        'prepare password
        'Console.Write("pass to be encrypted>")
        'Dim Pass1 = Console.ReadLine
        'Console.Write("encrypted string>")
        'Dim Pass2 = Console.ReadLine
        'Console.Write(EncryptString(Pass1, Pass2))
        'End
        Console.Write("Password>")
        Dim RegistryConfig = New RegistryClientConfiguration(My.Resources.URL)
        Dim AU As AuthenticationProvider = New PasswordOAuthAuthenticationProvider(My.Resources.Login, DecryptString(My.Resources.Pass, ReadPassword()))
        Dim Registry As Registry.IRegistryClient
        Try
            Registry = RegistryConfig.CreateClient(AU)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Dim Catalog As Models.Catalog = Registry.Catalog.GetCatalogAsync().Result
        Catalog.Repositories.ToList.ForEach(Sub(X)
                                                Dim Manifest As Models.GetImageManifestResult = Registry.Manifest.GetManifestAsync(X, "latest").Result
                                                Console.WriteLine($"{String.Format("{0,-20}", X)}  {Manifest.DockerContentDigest}")
                                            End Sub
                )
        Console.ReadLine()
    End Sub

#Region "console"
    Function ReadPassword() As String
        Dim Pass1 As New Text.StringBuilder
        While (True)
            Dim OneKey As ConsoleKeyInfo = Console.ReadKey(True)
            Select Case OneKey.Key
                Case = ConsoleKey.Enter
                    Return Pass1.ToString
                Case ConsoleKey.Backspace
                    If Pass1.Length > 1 Then
                        Pass1.Remove(Pass1.Length - 1, 1)
                        Console.Write(vbBack)
                    End If

                Case Else
                    If Not Char.IsControl(OneKey.KeyChar) Then
                        Pass1.Append(OneKey.KeyChar)
                        Console.Write("*")
                    End If
            End Select
        End While
    End Function
#End Region

End Module