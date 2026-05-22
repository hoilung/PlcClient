
using NewLife.Model;
using Opc;
using Opc.Ae;
using Opc.Da;
using Opc.Dx;
using Opc.Hda;
using OpcRcw;
using OpcRcw.Comn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace OpcCom
{
    public class ServerEnumerator2 : IDiscovery, IDisposable
    {
        private IOPCServerList2 m_server;

        private string m_host;

        private const string ProgID = "OPC.ServerList.1";

        private static readonly Guid CLSID;

        public void Dispose()
        {
        }

        public string[] EnumerateHosts()
        {
            return Interop.EnumComputers();
        }

        public Opc.Server[] GetAvailableServers(Specification specification)
        {
            return GetAvailableServers(specification, null, null);
        }

        public Opc.Server[] GetAvailableServers(Specification specification, string host, ConnectData connectData)
        {
            lock (this)
            {
                NetworkCredential credential = connectData?.GetCredential(null, null);
                m_server = (IOPCServerList2)Interop.CreateInstance(CLSID, host, credential);
                m_host = host;
                try
                {
                    ArrayList arrayList = new ArrayList();
                    Guid guid = new Guid(specification.ID);
                    IOPCEnumGUID ppenumClsid = null;
                    m_server.EnumClassesOfCategories(1, new Guid[1] { guid }, 0, null, out ppenumClsid);
                    Guid[] array = ReadClasses(ppenumClsid);
                    Interop.ReleaseServer(ppenumClsid);
                    ppenumClsid = null;
                    Guid[] array2 = array;
                    foreach (Guid clsid in array2)
                    {
                        Factory factory = new Factory();
                        try
                        {
                            URL url = CreateUrl(specification, clsid);
                            Opc.Server value = null;
                            if (specification == Specification.COM_DA_30)
                            {
                                value = new Opc.Da.Server(factory, url);
                            }
                            else if (specification == Specification.COM_DA_20)
                            {
                                value = new Opc.Da.Server(factory, url);
                            }
                            else if (specification == Specification.COM_AE_10)
                            {
                                value = new Opc.Ae.Server(factory, url);
                            }
                            else if (specification == Specification.COM_HDA_10)
                            {
                                value = new Opc.Hda.Server(factory, url);
                            }
                            else if (specification == Specification.COM_DX_10)
                            {
                                value = new Opc.Dx.Server(factory, url);
                            }

                            arrayList.Add(value);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    return (Opc.Server[])arrayList.ToArray(typeof(Opc.Server));
                }
                finally
                {
                    Interop.ReleaseServer(m_server);
                    m_server = null;
                }
            }
        }

        public Guid CLSIDFromProgID(string progID, string host, ConnectData connectData)
        {
            lock (this)
            {
                NetworkCredential credential = connectData?.GetCredential(null, null);
                m_server = (IOPCServerList2)Interop.CreateInstance(CLSID, host, credential);
                m_host = host;
                Guid clsid;
                try
                {
                    m_server.CLSIDFromProgID(progID, out clsid);
                }
                catch
                {
                    clsid = Guid.Empty;
                    return clsid;
                }
                finally
                {
                    Interop.ReleaseServer(m_server);
                    m_server = null;
                }

                return clsid;
            }
        }

        public ServerDescription[] GetServerDescriptions(Specification specification)
        {
            return GetServerDescriptions(specification, null, null);
        }

        public ServerDescription[] GetServerDescriptions(Specification specification, string host, ConnectData connectData)
        {
            lock (this)
            {
                NetworkCredential credential = connectData?.GetCredential(null, null);
                m_server = (IOPCServerList2)Interop.CreateInstance(CLSID, host, credential);
                m_host = host;
                try
                {
                    ArrayList arrayList = new ArrayList();
                    Guid guid = new Guid(specification.ID);
                    IOPCEnumGUID ppenumClsid = null;
                    m_server.EnumClassesOfCategories(1, new Guid[1] { guid }, 0, null, out ppenumClsid);
                    Guid[] array = ReadClasses(ppenumClsid);
                    Interop.ReleaseServer(ppenumClsid);
                    Guid[] array2 = array;
                    foreach (Guid clsid in array2)
                    {
                        arrayList.Add(ReadServerDetails(clsid));
                    }
                    return (ServerDescription[])arrayList.ToArray(typeof(ServerDescription));

                }
                catch (Exception)
                {
                    return new ServerDescription[0];
                }
                finally
                {
                    Interop.ReleaseServer(m_server);
                    m_server = null;
                }
            }
        }

        private Guid[] ReadClasses(IOPCEnumGUID enumerator)
        {
            ArrayList arrayList = new ArrayList();
            int pceltFetched = 0;
            int num = 10;
            IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Guid)) * num);
            try
            {
                do
                {
                    try
                    {
                        enumerator.Next(num, intPtr, out pceltFetched);
                        IntPtr ptr = intPtr;
                        for (int i = 0; i < pceltFetched; i++)
                        {
                            Guid guid = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
                            arrayList.Add(guid);
                            ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(Guid)));
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
                while (pceltFetched > 0);
                return (Guid[])arrayList.ToArray(typeof(Guid));
            }
            finally
            {
                Marshal.FreeCoTaskMem(intPtr);
            }
        }

        private URL CreateUrl(Specification specification, Guid clsid)
        {
            URL uRL = new URL();
            uRL.HostName = m_host;
            uRL.Port = 0;
            uRL.Path = null;
            if (specification == Specification.COM_DA_30)
            {
                uRL.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DA_20)
            {
                uRL.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DA_10)
            {
                uRL.Scheme = "opcda";
            }
            else if (specification == Specification.COM_DX_10)
            {
                uRL.Scheme = "opcdx";
            }
            else if (specification == Specification.COM_AE_10)
            {
                uRL.Scheme = "opcae";
            }
            else if (specification == Specification.COM_HDA_10)
            {
                uRL.Scheme = "opchda";
            }
            else if (specification == Specification.COM_BATCH_10)
            {
                uRL.Scheme = "opcbatch";
            }
            else if (specification == Specification.COM_BATCH_20)
            {
                uRL.Scheme = "opcbatch";
            }

            try
            {
                string ppszProgID = null;
                string ppszUserType = null;
                string ppszVerIndProgID = null;
                m_server.GetClassDetails(ref clsid, out ppszProgID, out ppszUserType, out ppszVerIndProgID);
                if (ppszProgID != null)
                {
                    uRL.Path = string.Format("{0}/{1}", ppszProgID, "{" + clsid.ToString() + "}");
                }
                else if (ppszVerIndProgID != null)
                {
                    uRL.Path = string.Format("{0}/{1}", ppszVerIndProgID, "{" + clsid.ToString() + "}");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (uRL.Path == null)
                {
                    uRL.Path = string.Format("{0}", "{" + clsid.ToString() + "}");
                }
            }

            return uRL;
        }

        private ServerDescription ReadServerDetails(Guid clsid)
        {
            ServerDescription serverDescription = new ServerDescription();
            serverDescription.HostName = this.m_host;
            serverDescription.Clsid = clsid;
            try
            {
                string ppszProgID = null;
                string ppszUserType = null;
                string ppszVerIndProgID = null;
                m_server.GetClassDetails(ref clsid, out ppszProgID, out ppszUserType, out ppszVerIndProgID);

                serverDescription.ProgId = ppszProgID;
                serverDescription.VersionIndependentProgId = ppszVerIndProgID;
                serverDescription.Description = ppszUserType;
            }
            catch
            {

            }
            return serverDescription;
        }
        static ServerEnumerator2()
        {
            CLSID = new Guid("13486D51-4821-11D2-A494-3CB306C10000");
        }
    }
}
