using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Model;
using Core.Infrastructure.Dev;
using Core.Services.Interface;
using Core.Data.UnitOfWork;

namespace Core.Services.Service
{
    public class LoggingService : ILogging
    {
        #region Members
        private WKTempUnitOfWork _UnitOfWork;

        #endregion

        public LoggingService(WKTempUnitOfWork _iUnitOfWork)
        {
            this._UnitOfWork = _iUnitOfWork;
        }

        public DevResponse LogDB(LogModel db_model)
        {
            DevResponse response = new DevResponse();
            try
            {
                this._UnitOfWork.Log(db_model);
                response.haveError = false;
            }
            catch (Exception ex1)
            {
                response.haveError = true;
                response.error = ex1.Message.ToString();
            }

            return response;
        }


        #region IDisposable Members

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion
    }
}
