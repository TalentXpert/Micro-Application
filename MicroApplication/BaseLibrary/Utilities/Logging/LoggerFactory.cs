using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseLibrary.Utilities
{
    public static class TxLoggerFactory
    {
        #region Members

        public static ILoggerFactory _currentLogFactory = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="logFactory">Log factory to use</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// Createt a new <paramref name="Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging.ILog"/>
        /// </summary>
        /// <returns>Created ILog</returns>
        public static ILogger CreateLogger<TCategory>()
        {
            if (_currentLogFactory != null)
                return _currentLogFactory.CreateLogger<TCategory>();
            return NullLogger.Instance;
            
        }

        #endregion
    }

    
}
