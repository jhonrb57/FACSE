using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metodos
{
    public class ClsNotificaciones
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsDistribuidor).Name);

        public List<Notificaciones> CargarNotificaciones(Guid idSucursal, Guid tipo)
        {
            try
            {
                var query = (tipo != Guid.Empty) ?
                _dbContext.adm_emisor_notificacion
                    .Where(x => x.eno_id_emisor == idSucursal && x.sys_tipo_notificacion.tno_id == tipo && x.eno_leido == false)
                    .AsEnumerable()
                    .Select(l => new Notificaciones
                    {
                        Descripcion = $"{l.eno_fecha} {l.eno_descripcion}",
                        Fecha = l.eno_fecha,
                        Id = l.eno_id,
                        Tipo = l.sys_tipo_notificacion.tno_id
                    }).OrderByDescending(x => x.Fecha).ToList() :
                     _dbContext.adm_emisor_notificacion
                    .Where(x => x.eno_id_emisor == idSucursal && x.eno_leido == false)
                    .AsEnumerable()
                    .Select(l => new Notificaciones
                    {
                        Descripcion = $"{l.eno_fecha} {l.eno_descripcion}",
                        Fecha = l.eno_fecha,
                        Id = l.eno_id,
                        Tipo = l.sys_tipo_notificacion.tno_id
                    }).OrderByDescending(x => x.Fecha).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid CaonsultarUltimaSucursal(Guid usuario)
        {
            try
            {
                var query = _dbContext.adm_usuario
                    .Where(x => x.usu_id == usuario)
                    .Select(x => x.usu_emisor_sucursal ?? Guid.Empty).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CambiarEstadoNotificacion(Guid idNot)
        {
            try
            {
                var query = _dbContext.adm_emisor_notificacion
                    .Where(x => x.eno_id == idNot)
                    .FirstOrDefault();

                query.eno_leido = true;

                _dbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cambiar todos los estados
        /// </summary>
        /// <param name="gEmisor"></param>
        /// <returns></returns>
        public bool CambiarTodosEstadoNotificacion(Guid gSucursal)
        {
            try
            {
                var query = _dbContext.adm_emisor_notificacion
                    .Where(x => x.eno_leido == false && x.eno_id_emisor == gSucursal);

                foreach (var item in query)
                {
                    item.eno_leido = true;
                }

                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
