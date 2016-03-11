
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

using System.Threading;
using System.Windows.Forms;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new MCMA().Unpack("McMyAdmin.ebi");
            File.WriteAllBytes("src.dll", x);
        }
    }




        public class MCMA 
        {
            private const string MCMA_URL = "http://www.mcmyadmin.com/Downloads/MCMA2-Latest.zip";

            private const string MCMA_BIN = "McMyAdmin.ebi";

            public string TempPath = "";

            private Ionic.Zlib.GZipStream DCStream = new Ionic.Zlib.GZipStream(new MemoryStream(), Ionic.Zlib.CompressionMode.Decompress);

            private byte[] EBIHeader = new byte[]
            {
            69,
            66,
            73,
            1
            };

            private IContainer components;

            public MCMA()
            {
               
            }

            protected  void OnShutdown()
            {
                
            }

            protected  void OnStart(string[] args)
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(this.BG_DoWork);
                backgroundWorker.RunWorkerAsync();
            }

            private void BG_DoWork(object sender, DoWorkEventArgs e)
            {
                this.ServiceMain();
            }

            protected  void OnStop()
            {
                Environment.Exit(0);
            }

            public void ServiceMain()
            {
                try
                {
                    Console.WriteLine("McMyAdmin Windows Service - {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Console.WriteLine();
                    this.TempPath = "MCMA_UPDATE.dat";
                    if (!File.Exists("McMyAdmin.ebi"))
                    {
                        this.GetMCMA();
                    }
                    int num;
                    do
                    {
                        num = this.StartMCMA();
                        if (num == -900)
                        {
                            this.GetMCMA();
                        }
                    }
                    while (num == -900);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Caught an exception of type '{0}'\n{1}\n", ex.GetType().Name, ex.Message);
                }
            }

            public bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            public void DownloadFileWithProgress(string URL, string Destination, string Caption)
            {
                Console.WriteLine(Caption);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.Validator);
                WebClient webClient = new WebClient();
                webClient.Proxy = null;
                webClient.Headers["User-Agent"] = "McMyAdmin-Updater";
                webClient.DownloadFileAsync(new Uri(URL), Destination);
                while (webClient.IsBusy)
                {
                    Thread.Sleep(100);
                }
                Console.WriteLine();
                Console.WriteLine("Download Complete\n");
                this.UnpackUpdate();
            }

            public void GetMCMA()
            {
                this.DownloadFileWithProgress("http://www.mcmyadmin.com/Downloads/MCMA2-Latest.zip", this.TempPath, "Downloading latest McMyAdmin release...");
            }

            public void DieFail()
            {
                Console.WriteLine("Failed to download main archive - please try again.");
                Environment.Exit(1);
            }

            public  byte[] Unpack(string filename)
            {
                byte[] array = File.ReadAllBytes(filename);
                if (array.Length == 0)
                {
                    this.DieFail();
                    File.Delete(filename);
                    return null;
                }
                for (int i = 0; i < this.EBIHeader.Length; i++)
                {
                    if (array[i] != this.EBIHeader[i])
                    {
                        return null;
                    }
                }
                MemoryStream memoryStream = new MemoryStream(array, false);
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                memoryStream.Seek(-12L, SeekOrigin.End);
                int num = binaryReader.ReadInt32() - 2;
                int num2 = binaryReader.ReadInt32();
                int num3 = binaryReader.ReadInt32();
                memoryStream.Seek((long)(-(long)(num + 20 - num3)), SeekOrigin.Current);
                int count = binaryReader.ReadInt32();
                int count2 = binaryReader.ReadInt32();
                memoryStream.Seek(0L, SeekOrigin.Begin);
                byte[] array2 = new byte[]
                {
                31,
                139
                };
                binaryReader.ReadBytes(4);
                byte[] signature = binaryReader.ReadBytes(count);
                byte[] keyBlob = binaryReader.ReadBytes(count2);
                memoryStream.Seek(8L, SeekOrigin.Current);
                byte[] array3 = binaryReader.ReadBytes(num);
                byte[] array4 = new byte[num + 2];
                array2.CopyTo(array4, 0);
                array3.CopyTo(array4, 2);
                byte[] array5 = new byte[num2];
                MemoryStream stream = new MemoryStream(array4);
                System.IO.Compression.GZipStream gZipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                gZipStream.Read(array5, 0, num2);
                try
                {
                    gZipStream.Close();
                }
                catch (Exception)
                {
                }
                RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                rSACryptoServiceProvider.ImportCspBlob(keyBlob);
                rSACryptoServiceProvider.VerifyData(array5, new SHA1CryptoServiceProvider(), signature);
                return array5;
            }

            public int StartMCMA()
            {
                int result;
                try
                {
                    if (File.Exists("McMyAdmin.ebi"))
                    {
                        byte[] array = this.Unpack("McMyAdmin.ebi");
                        bool flag = IntPtr.Size == 8;
                        if (!flag)
                        {
                            result = 1;
                            return result;
                        }
                        Console.WriteLine("Running in 64-bit mode.");
                        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MCMA.CurrentDomain_UnhandledException);
                        Assembly assembly = Assembly.Load(array, array);
                        object[] parameters = new object[]
                        {
                        new string[]
                        {
                            "-withUpdater",
                            "-asService"
                        }
                        };
                        while (true)
                        {
                            object obj = (int)assembly.EntryPoint.Invoke(null, parameters);
                            try
                            {
                                if (assembly.EntryPoint.ReturnType == typeof(int))
                                {
                                    int num = (int)obj;
                                    result = num;
                                    return result;
                                }
                                result = 1;
                                return result;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("MCMALDR Exception: " + ex.Message);
                                continue;
                            }
                            break;
                        }
                    }
                    Console.WriteLine("McMyAdmin failed to install properly.");
                    result = 1;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Unable to start McMyAdmin:");
                    string value = string.Concat(new string[]
                    {
                    "Exception in ",
                    ex2.Source,
                    "\n",
                    ex2.Message,
                    "\n",
                    ex2.StackTrace
                    });
                    Console.WriteLine(value);
                    result = 1;
                }
                return result;
            }

            private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                Exception ex = (Exception)e.ExceptionObject;
                Console.WriteLine("McMyAdmin Core Exception: " + ex.Message);
            }

            public void UnpackUpdate()
            {
                string[] source = new string[]
                {
                "Modern/css/Branding.css",
                "Modern/css/Branding.css.gz",
                "McMyAdmin.exe",
                "MCMA_Service.exe",
                "MCMA2_Linux_x86_64",
                "Modern/js/Config.js",
                "Modern/js/Config.js.gz",
                "Modern/js/Provider.js",
                "Modern/Images/BannerTemplate_custom.png",
                "Public/default.html"
                };
                ZipFile zipFile = new ZipFile(this.TempPath);
                foreach (ZipEntry current in zipFile.Entries)
                {
                    if (File.Exists(current.FileName))
                    {
                        if (source.Contains(current.FileName))
                        {
                            continue;
                        }
                    }
                    try
                    {
                        current.Extract(ExtractExistingFileAction.OverwriteSilently);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Extracting {0}\nError:{1}", current.FileName, ex.Message);
                    }
                }
                zipFile.Dispose();
                File.Delete(this.TempPath);
            }

          /*  internal int InstallService(bool Uninstall = false, bool Silent = false, ServiceStartMode startMode = ServiceStartMode.Manual)
            {
                int result;
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
                    DirectorySecurity accessControl = directoryInfo.GetAccessControl();
                    IdentityReference identity = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
                    FileSystemAccessRule rule = new FileSystemAccessRule(identity, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                    accessControl.AddAccessRule(rule);
                    directoryInfo.SetAccessControl(accessControl);
                    if (Uninstall)
                    {
                        ManagedInstallerClass.InstallHelper(new string[]
                        {
                        "/u",
                        Assembly.GetExecutingAssembly().Location
                        });
                        if (!Silent)
                        {
                            MessageBox.Show("Service Uninstalled Successfully.");
                        }
                    }
                    else
                    {
                        if (startMode == ServiceStartMode.Automatic)
                        {
                            File.WriteAllText("SVCAUTO", "SVCAUTO");
                        }
                        ManagedInstallerClass.InstallHelper(new string[]
                        {
                        Assembly.GetExecutingAssembly().Location
                        });
                        if (!Silent)
                        {
                            MessageBox.Show("Service Installed Successfully.");
                        }
                        if (File.Exists("SVCAUTO"))
                        {
                            File.Delete("SVCAUTO");
                        }
                    }
                    result = 0;
                }
                catch
                {
                    result = 1;
                }
                return result;
            }
            */
           
        }
    }


