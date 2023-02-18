using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UaiFuel.Models.ExceptionCore;

namespace UaiFuel.Models.Utils
{
    public class FrontUtil
    {
        public static string GetCssClass(AlertType alertType)
        {
            string bootstrapClass = "";
            if (AlertType.DANGER == alertType)
            { 
              bootstrapClass = "alert alert-danger";
            }
            else if(AlertType.WARNING == alertType)
            {
                bootstrapClass = "alert alert-warning";
            }
            else if (AlertType.INFO == alertType)
            {
                bootstrapClass = "alert alert-info";
            }
            else if (AlertType.SUCCESS == alertType)
            {
                bootstrapClass = "alert alert-success";
            }
            else if (AlertType.SECONDARY == alertType)
            {
                bootstrapClass = "alert alert-secondary";
            }
            
            return bootstrapClass;
        }

        public static void SetupViewBag(Controller controller, AppException exp)
        {
            controller.ViewBag.Message = exp.UserErrorDescription;
            controller.ViewBag.CssClass = GetCssClass(exp.Alert);
        }


        public static void SetupErrorsViewBag(Controller controller,
                                              IEnumerable<string> messages)
        {
            controller.ViewBag.CssClass = GetCssClass(AlertType.DANGER);
                
            string errorMessages = "<div class=\"" + GetCssClass(AlertType.DANGER) + "\"><ul>";
            
            foreach (string msg in messages.OrderBy(s => s))
            {
                errorMessages += "<li>" + msg +"</li>";
            }
            errorMessages += "</ul></div>";
            controller.ViewBag.ErrorMessages = errorMessages;
        }


        public static void AddToSession<T>(Controller controller, string key, T obj)
        {            
            controller.HttpContext.Session.SetString( key, JsonSerializer.Serialize<T>(obj) );
        }


        /**
         * Acessa um objeto serializado na sessão do usuário pela chave key.
         */
        public static T GetFromSession<T>(HttpContext context, string key)
        {
            string json = context.Session.GetString(key);
            if (json != null)
            {
                T obj = JsonSerializer.Deserialize<T>(json);
                return obj;
            }
            return default(T);

        }


        /**
         * Acessa e remove um objeto serializado na sessão do usuário.
         */
        public static T ExtractFromSession<T>(HttpContext context, string key)
        {
            string json = context.Session.GetString(key);
            if (json != null)
            {
                T obj = JsonSerializer.Deserialize<T>(json);
                context.Session.Remove(key);

                return obj;
            }
            return default(T);            
        }
    }
}

