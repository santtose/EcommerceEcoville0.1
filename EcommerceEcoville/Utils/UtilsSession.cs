using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceEcoville.Utils
{
    public class UtilsSession
    {
        private readonly IHttpContextAccessor _http;
        private readonly string CARINHO_ID = "carrinhoId";

        public UtilsSession(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string RetornarCarrinhoId()
        {
            if(_http.HttpContext.Session.GetString(CARINHO_ID) == null)
            {
                _http.HttpContext.Session.SetString(CARINHO_ID, Guid.NewGuid().ToString());
            }
            return _http.HttpContext.Session.GetString(CARINHO_ID);
        }
    }
}
