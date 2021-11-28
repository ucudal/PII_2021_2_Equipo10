using System;
using System.Collections;
using System.Collections.Generic;
using Ucu.Poo.Locations.Client;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Proyect
{
    /// <summary>
    /// Clase singleton para guardar los datos de la Aplicacion.
    /// Solo hay una instancia de esta clase, y es la que colabora con todas las demas.
    /// Esta clase, ademas guarda las instancias tanto de campanias y de emprendedores (La usa de manera cercana).
    /// </summary>
    public sealed class AppLogic
    {
        private LocationApiClient client = APILocationContainer.Instance.APIdeLocalizacion;
        private readonly static AppLogic _instance = new AppLogic();
        private List<Company> companies;
        private List<Emprendedor> entrepreneurs;
        private List<Rubro> validRubros = new List<Rubro>(){};//new Rubro("Alimentos"),new Rubro("Tecnologia"),new Rubro("Medicina")};

        private List<Qualifications> validQualifications = new List<Qualifications>(){};//new Qualifications("Vehiculo propio"),new Qualifications("Espacio para grandes volumenes de producto"),new Qualifications("Lugar habilitado para conservar desechos toxicos")};

        private List<Classification> validClasification = new List<Classification>(){};//new Classification("Organicos"),new Classification("Plasticos"),new Classification("Alimentos"),new Classification("Toxicos")};

        /// <summary>
        /// Obtiene las companias que estan registradas.
        /// </summary>
        /// <value>companies.</value>
        public List<Company> Companies
        {
            get{return this.companies;}
            set{this.companies = value;}
        }

        /// <summary>
        /// Obtiene los emprendedores que estan registrados.
        /// </summary>
        /// <value>entrepreneurs.</value>
        public List<Emprendedor> Entrepreneurs
        {
            get{return this.entrepreneurs;}
            set{this.entrepreneurs = value;}
        }

        /// <summary>
        /// Obtiene los rubros habilitados.
        /// </summary>
        /// <value>validRubros.</value>
        public List<Rubro> Rubros
        {
            get{return DeserializeRubros();}
        }

        /// <summary>
        /// Obtiene la lista de habilitaciones registradas.
        /// </summary>
        /// <value>validQualifications.</value>
        public List<Qualifications> Qualifications
        {
            get{return DeserializeQualifications();}
        }

        /// <summary>
        /// Obtiene la lista de clasificaciones/categorias registradas para los productos.
        /// </summary>
        /// <value>validClasification.</value>
        public List<Classification> Classifications
        {
            get{return DeserializeClasification();}
        }

        /// <summary>
        /// Convertidos a json.
        /// </summary>
        /// <returns></returns>
        public string ConvertToJson(List<Rubro> rubro)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Preserve,
                WriteIndented = true
            };

            return System.Text.Json.JsonSerializer.Serialize(rubro, options);
        }
        public string ConvertToJson(List<Qualifications> habilitaciones)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Preserve,
                WriteIndented = true
            };
            return System.Text.Json.JsonSerializer.Serialize(habilitaciones, options);
        }

        public string ConvertToJson(List<Classification> rubro)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Preserve,
                WriteIndented = true
            };

            return System.Text.Json.JsonSerializer.Serialize(rubro, options);
        }
        public string ConvertToJson(List<Emprendedor> rubro)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Preserve,
                WriteIndented = true
            };
            return System.Text.Json.JsonSerializer.Serialize(rubro, options);
        }

        public string ConvertToJson(List<Company> rubro)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = MyReferenceHandler.Preserve,
                WriteIndented = true
            };
            return System.Text.Json.JsonSerializer.Serialize(rubro, options);
        }

        public List<Rubro> DeserializeRubros()
        {
                string json = System.IO.File.ReadAllText(@"../Library/Persistencia/Rubros.json");
                return JsonConvert.DeserializeObject<List<Rubro>>(json);
        }
        public List<Qualifications> DeserializeQualifications()
        {
                string json = System.IO.File.ReadAllText(@"../Library/Persistencia/Habilitaciones.json");
                return JsonConvert.DeserializeObject<List<Qualifications>>(json);
        }
        public List<Classification> DeserializeClasification()
        {
                string json = System.IO.File.ReadAllText(@"../Library/Persistencia/ClasificacionesProductos.json");
                return JsonConvert.DeserializeObject<List<Classification>>(json);
        }
        public List<Emprendedor> DeserializeEntrenprenuers()
        {
                string json = System.IO.File.ReadAllText(@"../Library/Persistencia/Emprendedores.json");
                return JsonConvert.DeserializeObject<List<Emprendedor>>(json);
        }

        public List<Company> DeserializeCompanies()
        {
                string json = System.IO.File.ReadAllText(@"../Library/Persistencia/Companias.json");
                return JsonConvert.DeserializeObject<List<Company>>(json);
        }

        private AppLogic()
        {
            this.companies = new List<Company>();
            this.entrepreneurs = new List<Emprendedor>();
        }

        /// <summary>
        /// Obtiene la instancia de AppLogic.
        /// </summary>
        /// <value>_instance.</value>
        public static AppLogic Instance
        {
            get
            {
                return _instance;
            }
        }
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Emprendedor"/>.
        /// Utiliza el patron creator.
        /// </summary>
        /// <param name="name">El nombre del emprendedor.</param>
        /// <param name="ubication">La ubicacion del emprendedor.</param>
        /// <param name="rubro">El rubro del emprendedor.</param>
        /// <param name="habilitaciones">Las habilitaciones que tiene el emprendedor.</param>
        /// <param name="especializaciones">Las especializaciones que tiene el emprendedor.</param>
        /// <param name="user_Id">Id que tiene el emprendedor.</param> 
        public void RegisterEntrepreneurs(string user_Id, string name, string ubication, Rubro rubro, List<Qualifications> habilitaciones, string especializaciones, string user_Contact)
        {
            try
            {
                entrepreneurs.Add(new Emprendedor(user_Id, name, ubication, rubro, user_Contact, habilitaciones, especializaciones));
            }
            catch (EmptyUserBuilderException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Registra a una compania
        /// </summary>
        /// <param name="companyToken"></param>
        /// <param name="user_Id"></param>
        /// <param name="name"></param>
        /// <param name="ubication"></param>
        /// <param name="rubro"></param>
        /// <returns>mensaje de confirmacion</returns>
        public bool RegistrarCompany(string companyToken, string user_Id, string name, string ubication, Rubro rubro, string user_Contact)
        {
            Company company = Administrator.Instance.Invite(companyToken, user_Id, name, ubication, rubro, user_Contact);
            if (company == null)
            {
                return false;
            }else
            {
                Companies.Add(company);
                return true;
            }
        }

        /// <summary>
        /// Remueve palabras clave de la oferta de una compania.
        /// Le delega la responsabilidad a company (La experta).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="offer">La oferta.</param>
        /// <param name="keyWordIndex">La palabra clave.</param>
        public void RemoveKeyWords(Company company, IOffer offer, int keyWordIndex)
        {
            company.RemoveKeyWords(offer, keyWordIndex);
        }

        /// <summary>
        /// Agrega las palabras clave de una oferta.
        /// Le delaga la responsabilidad a Company (La epxerta).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="offer">La oferta.</param>
        /// <param name="keyWord">La palabra clave.</param>
        public bool AddKeyWords(Company company, IOffer offer, string keyWord)
        {
            return company.AddKeyWords(offer,keyWord);
        }

        /// <summary>
        /// Remueve la oferta de una compania.
        /// Le delega la responsabilidad a company (La experta).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="offerIndex">La oferta.</param>
        public void RemoveOffer(Company company, int offerIndex)
        {
            company.OffersPublished.RemoveAt(offerIndex);
        }

        /// <summary>
        /// Remueve las habilitaciones de una compania. 
        /// Le delega la responsabilidad a compani (La experta).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="offer">La oferta.</param>
        /// <param name="qualificationIndex">La habilitacion.</param>
        public void RemoveQualification(Company company, IOffer offer,int qualificationIndex)
        {
            company.RemoveQualification(offer, qualificationIndex);
        }

        /// <summary>
        /// Agrega habilitaciones a una oferta.
        /// Le delega la responsabilidad a company (La experta)
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="offer">La oferta.</param>
        /// <param name="qualification">La habilitacion.</param>
        public void AddQualification(Company company, IOffer offer, Qualifications qualification)
        {
            company.AddQualification(offer, qualification);
        }

        /// <summary>
        /// Publica una constante oferta de la compania que se le ingresa.
        /// ÑLe delaga la responsabilidad a company (Por patron creator).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="tipo">La clasificacion.</param>
        /// <param name="quantity">La cantidad.</param>
        /// <param name="cost">El precio.</param>
        /// <param name="ubication">La ubicacion.</param>
        /// <param name="qualifications">Las hablitaciones.</param>
        /// <param name="keyWords">La palabra clave.</param>
        public void PublicConstantOffer(Company company, Classification tipo, double quantity, double cost, string ubication, List<Qualifications> qualifications, ArrayList keyWords)
        {
            company.PublicConstantOffer(tipo,quantity,cost,ubication,qualifications,keyWords);
        }

        /// <summary>
        /// Publica una oferta no constnte de la compania que se le ingresa.
        /// Le delega la responsabilidad a company (Por patron creator).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <param name="tipo">La clasificacion.</param>
        /// <param name="quantity">La cantidad.</param>
        /// <param name="cost">El precio.</param>
        /// <param name="ubication">La ubicacion.</param>
        /// <param name="qualifications">Las hablitaciones.</param>
        /// <param name="keyWords">La palabra clave.</param>
        /// <returns>mensaje de confirmacion</returns>
        public void PublicNonConstantOffer(Company company, Classification tipo, double quantity, double cost, string ubication, List<Qualifications> qualifications, ArrayList keyWords)
        {
            company.PublicNonConstantOffer(tipo,quantity,cost,ubication,qualifications,keyWords);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por tipo.
        /// Le delega la responsabilidada a OfferSearch (Por SRP).
        /// </summary>
        /// <param name="word">Tipo de oferta.</param>
        /// <returns>Un ArrayList con todas las ofertas que sean de ese tipo.</returns>
        public List<IOffer> SearchOfferByType(string word)
        {
            return OfferSearch.Instance.SearchByType(word);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por ubicacion.
        /// Le delega la responsabilidada a OfferSearch (Por SRP).
        /// </summary>
        /// <param name="word">Ubicacion de la oferta.</param>
        /// <returns>Un ArrayList con todas las ofertas en la ubicacion dada.</returns>
        public List<IOffer> SearchOfferByUbication(string word)
        {
            return OfferSearch.Instance.SearchByUbication(word);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por palabra clave.
        /// Le delega la responsabilidada a OfferSearch (Por SRP).
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public List<IOffer> SearchOfferByKeywords(string keyWord)
        {
            return OfferSearch.Instance.SearchByKeywords(keyWord);
        }

        /// <summary>
        /// Metodo para aceptar una oferta.
        /// Como app logic es la que conoce todos los datos para habilitar la operacion, teien eesta responsabilidad.
        /// </summary>
        /// <param name="emprendedor">Emprendedor.</param>
        /// <param name="offer">Oferta a aceptar.</param>
        public bool AccepOffer(Emprendedor emprendedor, IOffer offer)
        {
            if (offer.PutBuyer(emprendedor))
            {
                emprendedor.AddPurchasedOffer(offer);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metodo que permite obtener la distancia entre un emprendedor y un producto.
        /// </summary>
        /// <param name="emprendedor">Emprendedor.</param>
        /// <param name="offer">La Oferta.</param>
        public async Task<string> ObteinOfferDistance(Emprendedor emprendedor, IOffer offer)
        {
            string emprendedorUbication = emprendedor.Ubication;
            string offerUbication = offer.Product.Ubication;
            Location locationEmprendedor = await client.GetLocationAsync(emprendedorUbication);
            Location locationOffer = await client.GetLocationAsync(offerUbication);
            Distance distance = await client.GetDistanceAsync(locationEmprendedor, locationOffer);
            double kilometers = distance.TravelDistance;
            await client.DownloadRouteAsync(locationEmprendedor.Latitude, locationEmprendedor.Longitude,
            locationOffer.Latitude, locationOffer.Longitude, @"route.png");
            return Convert.ToString(kilometers);
        }

        /// <summary>
        /// Metodo que obtiene el mapa de la ubicacion de una oferta.
        /// </summary>
        /// <param name="offer">Oferta que se quiere buscar.</param>
        /// <returns>Un mapa de la ubicacion de la oferta.</returns>
        public async Task ObteinOfferMap(IOffer offer)
        {
            string offerUbication = offer.Product.Ubication;
            Location locationOffer = await client.GetLocationAsync(offerUbication);
            await client.DownloadMapAsync(locationOffer.Latitude, locationOffer.Longitude, @"map.png");
        }

        /// <summary>
        /// Metodo que devuelve un string con la lista de materiales constantes.
        /// Por expert tiene esta responsabilidad.
        /// </summary>
        /// <returns>Un string con aquellos materiales que son recuerrentes.</returns>
        public Dictionary<Classification, int> GetConstantMaterials()
        {

            Dictionary<Classification, int> clasificationDictionary = new Dictionary<Classification, int>();
            ArrayList constantMaterials = new ArrayList();

            foreach(Classification item in Classifications)
            {
                clasificationDictionary.Add(item,0);
            }
            foreach (Company company in Companies)
            {
                foreach (IOffer offer in company.OffersPublished)
                {
                    if (offer.GetType().Equals(typeof(ConstantOffer)))
                    {
                        constantMaterials.Add(offer.Product);
                        clasificationDictionary[offer.Product.Classification] += 1;
                    }
                }
            }
            return clasificationDictionary;
        }

        /// <summary>
        /// Obtiene un string con la indicando si sus ofertas fueron o no fueron aceptadas, en caso de que si, indica ademas la fecha de cuando fueron aceptadas.
        /// Le delega la responsabilidad a Company (La experta).
        /// </summary>
        /// <param name="company">La compania.</param>
        /// <returns>Un string con las ofertas que fueron aceptadas.</returns>
        public List<IOffer> GetOffersAccepted(Company company)
        {
            return company.GetOffersAccepted();
        }

        /// <summary>
        /// Obtiene las ofertas aceptadas por el emprendedor, junto con la fecha de cuando las acepto.
        /// Le delega la responsabilidad a emprendedor, la experta.
        /// Es una operacion polimorfica.
        /// </summary>
        /// <param name="emprendedor">Emprendedor.</param>
        /// <returns>Un string con las ofertas que fueron aceptadas.</returns>
        public List<IOffer> GetOffersAccepted(Emprendedor emprendedor)
        {
            return emprendedor.PurchasedOffers;
        }

        /// <summary>
        /// Obtiene la cantidad de ofertas que fueron aceptadas en un periodo de tiempo establecido por el usuario.
        /// Le delega la responsabilidad a company (La experta).
        /// Es una operacion polimorfica.
        /// </summary>
        /// <param name="company">Compania.</param>
        /// <param name="periodTime">Periodo de tiempo establecido por el usuario.</param>
        /// <returns></returns>
        public List<IOffer> GetOffersAccepted(Company company, int periodTime)
        {
            return company.GetOffersAccepted(periodTime);
        }

        /// <summary>
        /// Obtiene la cantidad de ofertas que fueron aceptadas en un periodo de tiempo establecido por el usuario.
        /// Le delega la responasabilidad a emprendedor (La experta).
        /// Es una operaciion polimorfica.
        /// </summary>
        /// <param name="emprendedor">Emprendedor.</param>
        /// <param name="periodTime">Periodo de tiempo establecido por el usuario.</param>
        /// <returns></returns>
        public List<IOffer> GetOffersAccepted(Emprendedor emprendedor, int periodTime)
        {
            return emprendedor.GetOffersAccepted(periodTime);
        }

        /// <summary>
        /// Obtiene la compania mediente el id de usuraui ingreado.
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public Company GetCompany(string user_Id)
        {
            foreach(Company item in this.Companies)
            {
                if (item.User_Id == user_Id)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene el emprendedor mendiante el ingreso de un id de usuario.
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public Emprendedor GetEmprendedor(string user_Id)
        {
            foreach (Emprendedor item in this.Entrepreneurs)
            {
                if (item.User_Id == user_Id)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
