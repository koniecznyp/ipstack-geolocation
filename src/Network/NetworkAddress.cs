using System;
using System.Net;
using System.Text.RegularExpressions;
using Network.Interfaces;
using Services.Exceptions;

namespace Network
{
    public class NetworkAddress : INetworkAddress
    {
        public bool CheckIfAddressIsValid(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ServiceException(HttpStatusCode.BadRequest, "Empty address");
            }
            return true;
        }
    }
}