using System;
using System.Linq;
using Telegram.Bot.Types;

namespace Proyect
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility.
    /// </summary>
    public abstract class BaseHandler : IHandler
    {
        /// <summary>
        /// Obtiene el próximo "handler".
        /// </summary>
        /// <value>El "handler" que será invocado si este "handler" no procesa el mensaje.</value>
        public IHandler Next { get; set; }

        /// <summary>
        /// Obtiene o asigna el conjunto de palabras clave que este "handler" puede procesar.
        /// </summary>
        /// <value>Un array de palabras clave.</value>
        public string[] Keywords { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/>.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(IHandler next)
        {
            this.Next = next;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseHandler"/> con una lista de comandos.
        /// </summary>
        /// <param name="keywords">La lista de comandos.</param>
        /// <param name="next">El próximo "handler".</param>
        public BaseHandler(string[] keywords, BaseHandler next)
        {
            this.Keywords = keywords;
            this.Next = next;
        }

        /// <summary>
        /// Este método debe ser sobreescrito por las clases sucesores. La clase sucesora procesa el mensaje y retorna
        /// true o no lo procesa y retorna false.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="responder">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario</returns>
        protected abstract bool InternalHandle(IMessage message, out string responder);

        /// <summary>
        /// Este método puede ser sobreescrito en las clases sucesores que procesan varios mensajes cambiando de estado
        /// entre mensajes.
        /// </summary>
        protected virtual void InternalCancel()
        {
        }

        /// <summary>
        /// Determina si este "handler" puede procesar el mensaje. En la clase base se utiliza el array
        /// <see cref="BaseHandler.Keywords"/> para buscar el texto en el mensaje ignorando mayúsculas y minúsculas. Las
        /// clases sucesores pueden sobreescribir este método para proveer otro mecanismo para determina si procesan o no
        /// un mensaje.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <returns>true si el mensaje puede ser pocesado; false en caso contrario.</returns>
        protected virtual bool CanHandle(IMessage message)
        {
            if (this.Keywords == null || this.Keywords.Length == 0)
            {
                throw new InvalidOperationException("No hay palabras clave que puedan ser procesadas");
            }

            return this.Keywords.Any(s => message.Text.Equals(s, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Procesa el mensaje o la pasa al siguiente "handler" si existe.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>El "handler" que procesó el mensaje si el mensaje fue procesado; null en caso contrario.</returns>
        public IHandler Handle(IMessage message, out string response)
        {
            if (this.InternalHandle(message, out response))
            {
                return this;
            }
            else if (this.Next != null)
            {
                return this.Next.Handle(message, out response);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna este "handler" al estado inicial. En los "handler" sin estado no hace nada. Los "handlers" que
        /// procesan varios mensajes cambiando de estado entre mensajes.
        /// 
        /// </summary>
        public virtual void Cancel()
        {
            this.InternalCancel();
            if (this.Next != null)
            {
                this.Next.Cancel();
            }
        }
    }
}