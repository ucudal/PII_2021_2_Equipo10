using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Proyect
{
    /// <summary>
    /// Clase base para implementar el patrón Chain of Responsibility.
    /// </summary>
    public class CompanyMyOfferHandler : BaseHandler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CompanyMyOfferHandler"/>. Esta clase elimina la oferta publicada de una compania.
        /// </summary>
        /// <param name="next">El próximo "handler".</param>
        public CompanyMyOfferHandler(BaseHandler next) : base(next)
        {
            this.Keywords = new string[] {"/misofertas","/removeroferta","/removerkeyword","/removerhabilitacion","/agregarkeyword","/agregarhabilitacion","/oferta"};
        }

        /// <summary>
        /// Procesa el mensaje "/registrar" y retorna true; retorna false en caso contrario.
        /// </summary>
        /// <param name="message">El mensaje a procesar.</param>
        /// <param name="response">La respuesta al mensaje procesado.</param>
        /// <returns>true si el mensaje fue procesado; false en caso contrario.</returns>
        protected override bool InternalHandle(IMessage message, out string response)
        {
            if (message.Text.ToLower().Replace(" ","").Equals("/misofertas"))
            {
                Company compania = null;
                foreach(Company item in AppLogic.Instance.Companies)
                {
                    if (item.User_Id.Equals(message.Id))
                    {
                        compania = item;
                    }
                }
                if(compania == null)
                {
                    response = "Este comando esta exclusivo de aquellos resgistrados como compania.";
                    return true;
                }
                if(!DataUserContainer.Instance.UserDataHistory.Keys.Contains(message.Id))
                {
                    StringBuilder mensaje = new StringBuilder();
                    mensaje.Append("Esta es la lista de sus ofertas publicadas.\n");
                    int index = 0;
                    foreach(IOffer item in compania.OffersPublished)
                    {
                        index++;
                        mensaje.Append($"{index}--{item.Product.Quantity} Kilos de {item.Product.Classification.Category} a un precio de {item.Product.Price}$ en {item.Product.Ubication} (Publicada el {item.DatePublished})\n");
                    }
                    mensaje.Append("\nUsted puede utilizar:\n-/RemoverOferta para eliminar una oferta\n-/AgregarKeyWord para agregar una palabra clave a la oferta\n-/RemoverKeyWord para sacar una palabra clave de la oferta\n-/AgregarHabilitacion para agregar habilitaciones a la oferta\n/RemoverHabilitacion para eliminar una habilitacion a la oferta");
                    response = mensaje.ToString();
                    List<List<string>> lista = new List<List<string>>() {new List<string>(),new List<string>()};
                    DataUserContainer.Instance.UserDataHistory.Add(message.Id,lista);
                    DataUserContainer.Instance.UserDataHistory[message.Id][0].Add("/misofertas");
                    return true;
                }else
                {
                    response = "Usted ya esta en proceso de modificacion de ofertas";
                    return true;
                }
            }if(DataUserContainer.Instance.UserDataHistory.Keys.Contains(message.Id.ToLower().Replace(" ","")) && DataUserContainer.Instance.UserDataHistory[message.Id][0][0].Equals("/misofertas"))
            {
                List<string> userData = DataUserContainer.Instance.UserDataHistory[message.Id][1];
                Company compania = null;
                foreach(Company item in AppLogic.Instance.Companies)
                {
                    if (item.User_Id.Equals(message.Id))
                    {
                        compania = item;
                    }
                }
                string mensaje = message.Text.Trim(' ');
                string[] comando = mensaje.ToLower().Split(" ");
                if (comando.Count() == 2 | userData.Count != 0)
                {
                    if (this.Keywords.Contains(comando[0]) | DataUserContainer.Instance.UserDataHistory[message.Id][0].Count >= 2)
                    {
                        string indiceIngresado = "";
                        bool ofertaCase = false; 
                        if(DataUserContainer.Instance.UserDataHistory[message.Id][0].Contains("/oferta") & DataUserContainer.Instance.UserDataHistory[message.Id][0].Count == 2)
                        {
                            if(comando.Count() >= 2)
                            {
                                response = "Solo se admiten comandos unicos en esta instancia";
                                return true;
                            }
                            else
                            {
                                ofertaCase = true;
                                indiceIngresado = (Convert.ToInt32(userData[0])+1).ToString();
                                userData.RemoveAt(0);
                            }
                        }
                        switch(userData.Count)
                        {
                            case 0:
                                if (!ofertaCase)
                                {
                                    indiceIngresado = comando[1];
                                }
                                int number;
                                if (int.TryParse(indiceIngresado, out number))
                                {
                                    if (compania.OffersPublished.Count - number >= 0)
                                    {
                                        DataUserContainer.Instance.UserDataHistory[message.Id][0].Add(comando[0]);
                                        number--;
                                        userData.Add(number.ToString());
                                        IOffer oferta = compania.OffersPublished[number];
                                        StringBuilder mensajes = new StringBuilder();
                                        if (comando[0].Equals("/oferta"))
                                        {
                                            foreach(Qualifications item in oferta.Qualifications)
                                            {
                                                mensajes.Append($"\n-{item.QualificationName}");
                                            }
                                            StringBuilder mensajePalabras = new StringBuilder();
                                            foreach(string item in oferta.KeyWords)
                                            {
                                                mensajePalabras.Append($"|{item}|");
                                            }
                                            response = $"Oferta {number+1}.\nClasificacion del producto: {oferta.Product.Classification}\nCantidad: {oferta.Product.Quantity} Kilogramos\nPrecio: {oferta.Product.Price}$\nUbicacion: {oferta.Product.Ubication}\nHabilitaciones: {mensajes}\nPalabras clave: {mensajePalabras}";
                                            return true;
                                        }if (comando[0].Equals("/removeroferta"))
                                        {
                                            response = $"Esta Seguro que quiere eliminar la oferta de --{oferta.Product.Quantity} Kilos de {oferta.Product.Classification} a un precio de {oferta.Product.Price} en {oferta.Product.Ubication} (Publicada el {oferta.DatePublished})";
                                            return true;
                                        }if (comando[0].Equals("/removerkeyword"))
                                        {
                                            int index = 0;
                                            foreach(string item in oferta.KeyWords)
                                            {
                                                index++;
                                                mensajes.Append($"\n{index}-{item}");
                                            }
                                            response = $"Las palabra/s clave de la oferta de: \n--{oferta.Product.Quantity} Kilos de {oferta.Product.Classification.Category} a un precio de {oferta.Product.Price}$ en {oferta.Product.Ubication} (Publicada el {oferta.DatePublished})\nSon: {mensajes}\nPor favor seleccione el indice de la que quiere eliminar.";
                                            return true;
                                        }if (comando[0].Equals("/removerhabilitacion") )
                                        {
                                            if (oferta.Qualifications.Count != 1)
                                            {
                                                int index = 0;
                                                foreach(Qualifications item in oferta.Qualifications)
                                                {
                                                    index++;
                                                    mensajes.Append($"\n{index}-{item.QualificationName}");
                                                }
                                                response = $"Las habiliatciones de la oferta de --{oferta.Product.Quantity} Kilos de {oferta.Product.Classification} a un precio de {oferta.Product.Price} en {oferta.Product.Ubication} (Publicada el {oferta.DatePublished})\nSon {mensaje}\nPor favor seleccione el indice de la que quiere eliminar.";
                                                return true;
                                            }else
                                            {
                                                response = "La oferta seleccionada solo posee una habilitacion. Para remover habilitaciones de una oferta se necesitan minimo dos.\n\n(Se regresara al estado final)";
                                                DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                                return true;
                                            }
                                        }if (comando[0].Equals("/agregarhabilitacion"))
                                        { 
                                            int index = 0;
                                            mensajes.Append("Por favor, seleccione la habilitacion que desea agregar (indique el indice)\n");
                                            foreach(Qualifications item in AppLogic.Instance.Qualifications)
                                            {
                                                index++;
                                                mensajes.Append($"\n{index}-{item.QualificationName}");
                                            }
                                            response = mensajes.ToString();
                                            return true;
                                        }else
                                        {
                                            int index = 0;
                                            mensajes.Append($"Estan son las actuales palabras claves de la oferta seleccionada (oferta {Convert.ToInt32(userData[0])+1})\n");
                                            foreach(string item in oferta.KeyWords)
                                            {
                                                index++;
                                                mensajes.Append($"\n{index}-{item}");
                                            }
                                            mensajes.Append("\n\nEscriba las nuevas palabras clave a agregar");
                                            response = mensajes.ToString();
                                            return true;
                                        }

                                    }else
                                    {
                                        response = "Numero invalido";
                                        return true;
                                    }
                                }else
                                {
                                    response = "El dato ingresado no es valido\nPor favor, revise que haya ingresado un numero (Ej:'1' Para elegir la primera oferta)";
                                    return true;
                                }
                            case 1:
                                if (DataUserContainer.Instance.UserDataHistory[message.Id][0].Contains("/removeroferta"))
                                {
                                    if ((message.Text.ToLower().Replace(" ","").Equals("/si")))
                                    {
                                        compania.OffersPublished.RemoveAt(Convert.ToInt32(userData[0]));
                                        DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                        response = "Se a removido la oferta";
                                        return true;
                                    }if ((message.Text.ToLower().Replace(" ","").Equals("/no")))
                                    {
                                        DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                        response = "Se ha cancelado la eliminacion de la oferta";
                                        return true;
                                    }
                                    response = "Debe ingresr /si o /no";
                                }if (!DataUserContainer.Instance.UserDataHistory[message.Id][0].Contains("/agregarkeyword")) 
                                {
                                    if (int.TryParse(message.Text, out number))
                                    {
                                        if (DataUserContainer.Instance.UserDataHistory[message.Id][0].Contains("/removerkeyword"))
                                        {
                                            if (compania.OffersPublished[Convert.ToInt32(userData[0])].KeyWords.Count - number >= 0)
                                            {
                                                response = $"Las palabra clave {compania.OffersPublished[Convert.ToInt32(userData[0])].KeyWords[number-1]} se removio de la oferta seleccionada";
                                                compania.OffersPublished[Convert.ToInt32(userData[0])].KeyWords.RemoveAt(number -1);
                                                DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                                return true;
                                            }else
                                            {
                                                response = "Numero invalido";
                                            }
                                        }if (DataUserContainer.Instance.UserDataHistory[message.Id][0].Contains("/removerhabilitaciones"))
                                        {
                                            if (compania.OffersPublished[Convert.ToInt32(userData[0])].Qualifications.Count - number >= 0)
                                            {
                                                DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                                response = $"La habiliatcion {compania.OffersPublished[Convert.ToInt32(userData[0])].Qualifications[number-1]} se removio de la oferta seleccionada";
                                                compania.OffersPublished[Convert.ToInt32(userData[0])].Qualifications.RemoveAt(number-1);
                                                return true;
                                            }else
                                            {
                                                response = "Numero invalido";
                                            }
                                        }else
                                        {
                                            if (AppLogic.Instance.Qualifications.Count - number >= 0)
                                            {
                                                if (!compania.OffersPublished[Convert.ToInt32(userData[0])].Qualifications.Contains(AppLogic.Instance.Qualifications[number-1]))
                                                {
                                                    DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                                    compania.OffersPublished[Convert.ToInt32(userData[0])].Qualifications.Add(AppLogic.Instance.Qualifications[number-1]);
                                                    response = $"se agrego {AppLogic.Instance.Qualifications[number-1].QualificationName} a la oferta seleccionada";
                                                    
                                                }else
                                                {
                                                    response = "La habilitacion ya se encuentra en la oferta";
                                                    
                                                }
                                            }else
                                            {
                                                response = "Numero invalido";
                                                
                                            }
                                        }
                                    }else
                                    {
                                        response = "El dato ingresado no es valido\nPor favor, revise que haya ingresado un numero (Ej:'1' Para elegir la primera palabra clave)";
                                    }
                                }else
                                {
                                    if (!message.Text.ToLower().Replace(" ","").Equals("/stop"))
                                    {
                                        if (!string.IsNullOrEmpty(message.Text.ToLower()) || !message.Text.ToLower().Contains("?"))
                                        {
                                            if (!compania.OffersPublished[Convert.ToInt32(userData[0])].KeyWords.Contains(message.Text))
                                            {
                                                compania.OffersPublished[Convert.ToInt32(userData[0])].KeyWords.Add(message.Text);
                                                response = $"Se agrego {message.Text} como palabra clave a la oferta.\n\nPuede agregar otra o hacer /stop para terminar el proceso";
                                            }else
                                            {
                                                response = $"{message.Text} ya es una palabra clave de la oferta seleccionada";
                                            }
                                        }else
                                        {
                                            response = "El dato ingresado no es valido";
                                        }
                                    }else
                                    {
                                        DataUserContainer.Instance.UserDataHistory.Remove(message.Id);
                                        response = "se termino de agregar palabras clave";
                                    }
                                }

                            return true;
                        }
                    }
                    response = "El comando ingresado no es valido.\nRecuerde que puede utilizar: \n-/RemoverOferta para eliminar una oferta\n-/AgregarKeyWord para agregar una palabra clave a la oferta\n-/RemoverKeyWord para sacar una palabra clave de la oferta\n-/AgregarHabilitacion para agregar habilitacion a la oferta\n/ReomverHabilitacion para eliminar una habilitacion a la oferta";
                    return true;
                }

                response = "Debe ingresar El comando y el indice (Ej: RemoverOferta 1 para remover la primera oferta)";
                return true;
            }
            response = string.Empty;
            return false;
        }
    }
}