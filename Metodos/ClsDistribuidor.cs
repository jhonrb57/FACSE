using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metodos
{
    public class ClsDistribuidor
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsDistribuidor).Name);

        /// <summary>
        /// informacion de emiosr
        /// </summary>
        /// <param name="gDistribuidor"></param>
        /// <returns></returns>
        public adm_distriduidor InformacionDistribuidor(Guid gDistribuidor)
        {
            try
            {
                return _dbContext.adm_distriduidor.AsNoTracking().Where(d => d.dis_id == gDistribuidor).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
