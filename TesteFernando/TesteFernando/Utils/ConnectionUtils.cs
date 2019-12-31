using Plugin.Connectivity;
using System;

namespace TesteFernando.Utils
{
    public static class ConnectionUtils
    {
        public static bool IsConnected()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        /// <summary>
        /// Valida se está conectado e dispara exception se não estiver
        /// </summary>
        public static void ValidateConnection()
        {
            if (!IsConnected())
                throw new Exception("Verifique a conexão");
        }

    }
}
