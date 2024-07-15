using DataBase;
using log4net;
using System;
using System.Linq;

namespace Metodos
{
    public class ClsReceptor
    {
        FacseEntity _dbContext = new FacseEntity();
        public static readonly ILog log = LogManager.GetLogger(typeof(ClsReceptor).Name);
        private readonly ClsFuncion _clsFuncion = new ClsFuncion();

        /// <summary>
        /// validar si existe el receptor con su contrasena
        /// </summary>
        /// <param name="sUsuario"></param>
        /// <param name="sContrasena"></param>
        /// <returns></returns>
        public adm_usuario_receptor ValidarReceptor(string sUsuario, string sContrasena)
        {
            try
            {
                string sContrasenaEncriptada = _clsFuncion.Encriptar(sContrasena);
                var usuarioReceptor = from r in _dbContext.adm_usuario_receptor.AsNoTracking()
                               where r.ure_usuario.Equals(sUsuario) && r.ure_contrasena.Equals(sContrasenaEncriptada) && r.ure_activo == true
                               select r;

                return (usuarioReceptor.FirstOrDefault());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
