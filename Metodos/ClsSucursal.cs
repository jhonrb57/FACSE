using DataBase;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metodos
{
    public class ClsSucursal
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsSucursal).Name);

        /// <summary>
        /// cargar listado sucursal
        /// </summary>
        /// <returns></returns>
        public List<adm_emisor_sucursal> ListadoEmisorSucursal(Guid gUsuario)
        {
            try
            {
                return _dbContext.adm_usuario_sucursal.AsNoTracking()
                    .Where(es => es.usu_usuario == gUsuario && es.usu_activo == true && es.adm_emisor_sucursal.esu_activo == true).AsEnumerable()
                    .Select(u => u.adm_emisor_sucursal).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// validar si tienen sucursales los emisores
        /// </summary>
        /// <param name="gUsuario"></param>
        /// <param name="iEmisor"></param>
        /// <returns></returns>
        public Guid? ValidarEmisorSucursal(Guid gUsuario, Guid? gEmisor)
        {
            try
            {
                var usuarioEmisor = _dbContext.adm_usuario_sucursal.AsNoTracking().Where(ues => ues.usu_usuario == gUsuario && ues.adm_emisor_sucursal.esu_emisor == gEmisor && ues.adm_emisor_sucursal.esu_activo == true);
                if (usuarioEmisor.Any())
                {
                    if (_dbContext.adm_usuario.AsNoTracking().Where(u => u.usu_id == gUsuario && u.usu_emisor_sucursal != null).Any())
                    {
                        return _dbContext.adm_usuario.AsNoTracking().Where(u => u.usu_id == gUsuario).FirstOrDefault().usu_emisor_sucursal;
                    }
                    else
                    {
                        var emisorSucursal = usuarioEmisor.FirstOrDefault().usu_sucursal;
                        foreach (var item in _dbContext.adm_usuario.Where(u => u.usu_id == gUsuario))
                        {
                            item.usu_emisor_sucursal = emisorSucursal;
                        }
                        _dbContext.SaveChanges();
                        return emisorSucursal;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// cambiar usuario sucursal
        /// </summary>
        /// <param name="gSucursal"></param>
        /// <param name="gUsuario"></param>
        public void CambioSucursalUsuario(Guid gSucursal, Guid gUsuario)
        {
            try
            {
                foreach (var item in _dbContext.adm_usuario.Where(u => u.usu_id == gUsuario))
                {
                    item.usu_emisor_sucursal = gSucursal;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
