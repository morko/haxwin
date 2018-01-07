using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Titanium.Web.Proxy.EventArguments;

namespace HttpProxy
{
    public class Proxy
    {
        ProxyServer proxyServer;
        public void Start()
        {
            this.proxyServer = new ProxyServer();

            //locally trust root certificate used by this proxy 
            this.proxyServer.TrustRootCertificate = false;

            this.proxyServer.BeforeRequest += this.OnRequest;

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Parse("127.0.0.1"), 8080, false);

            //An explicit endpoint is where the client knows about the existence of a proxy
            //So client sends request in a proxy friendly manner
            this.proxyServer.AddEndPoint(explicitEndPoint);
            this.proxyServer.Start();

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            //Only explicit proxies can be set as system proxy!
            this.proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
        }
        public void Stop() {
            //Unsubscribe & Quit
            this.proxyServer.BeforeRequest -= this.OnRequest;

            this.proxyServer.Stop();
        }
        //To access requestBody from OnResponse handler
        private IDictionary<Guid, string> requestBodyHistory
                = new ConcurrentDictionary<Guid, string>();

        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            ////read request headers
            var requestHeaders = e.WebSession.Request.Headers;
            requestHeaders.AddHeader("Referer", "http://www.haxball.com/haxball20.swf");
        }
    }
}
