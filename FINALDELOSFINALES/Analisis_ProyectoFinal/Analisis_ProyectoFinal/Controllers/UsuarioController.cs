using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Analisis_ProyectoFinal.Models;

namespace Analisis_ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            usuario userModel = new usuario();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult AddOrEdit(usuario userModel /*,int rol_seleccion*/)
        {

            try
            {
                using (ModelsAnalisis dbModel = new ModelsAnalisis())
                {
                    usuario usuarioModel = new usuario();
                    rol_usuario rol_ = new rol_usuario();
                    String tipo_Usuario = "Cliente";
                    //switch (rol_seleccion)
                    //{
                    //    case 1:
                    //        tipo_Usuario = "Cliente";
                    //        break;

                    //    case 2:
                    //        tipo_Usuario = "Jefe_Administrador";
                    //        break;
                    //    case 3:
                    //        tipo_Usuario = "Jefe_Financiero";
                    //        break;
                    //}
                    usuarioModel.nombre = userModel.nombre;
                    usuarioModel.apellido = userModel.apellido;
                    usuarioModel.fecha_ingreso = userModel.fecha_ingreso;
                    usuarioModel.username = userModel.username;
                    usuarioModel.contraseña = userModel.contraseña;
                    usuarioModel.rol = dbModel.rol_usuario.Where(o => o.nombre_rol == "Cliente").FirstOrDefault().id_rol;

                    ;

                    dbModel.usuarios.Add(usuarioModel);
                    dbModel.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.SuccesMessage = "Registro Exitoso";
                return View("AddOrEdit", new usuario());
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    
            [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(usuario userCredential)
        {
            using (ModelsAnalisis userModel = new ModelsAnalisis())
            {
                var userDetail = userModel.usuarios.Where(x => x.username == userCredential.username && x.contraseña == userCredential.contraseña).FirstOrDefault();
                if (userDetail == null)
                {
                    userCredential.loginErrorMessage = "Credenciales incorrectas";
                    return View("Login", userCredential);
                }
                else
                {
                    Session["id_usuario"] = userDetail.id_usuario;
                    return RedirectToAction("IndexView");
                }


                return View("Login");
            }

        }


    }


    }



