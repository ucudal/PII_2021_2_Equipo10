using System.Threading.Tasks;

namespace Proyect
{
    /// <summary>
    /// Interfaz para los mensajes, que se utilizara para el patron adapter.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Id del usuario.
        /// </summary>
        /// <value>El id.</value>
        string Id { get;set;}

        /// <summary>
        /// El mensaje.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Id del chat.
        /// </summary>
        /// <value>El chat id.</value>
        long MsgId{get;set;}

        /// <summary>
        /// Envia una imagen a un usuario.
        /// Esto es paar segrui con el patroon adapter, y tener el encapsulamiento lo mejor posible.
        /// </summary>
        /// <param name="mensaje">El mesaje que contendra la imegan.</param>
        /// <param name="direccion">La direcion del chat a que se le enciara.</param>
        /// <returns></returns>
        Task SendProfileImage(string mensaje, string direccion);
    }
}