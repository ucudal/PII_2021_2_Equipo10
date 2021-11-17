using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Proyect
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility.
    /// </summary>
    public abstract class RegisterHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RegisterHandler"/>. Esta clase procesa el mensaje "/Registrar" de un usuario.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public RegisterHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new string[] {"/registrar"};
        }

        /// <summary>
        /// Procesa el mensaje "/registrar" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override bool InternalHandle(Message message, out string response)
        {

            if (message.Text.ToLower().Equals("/registrar"))
            {
                response = "Bienvenido a C4BOT\n¿Tenes Token?";
                return true;
            }if (DataUserContainer.Instance.UserDataHistory[])

            response = string.Empty;
            return false;
        }
    }
}