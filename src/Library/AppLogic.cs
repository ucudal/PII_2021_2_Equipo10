using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using LocationApi;
using System.Threading.Tasks;

namespace Proyect
{
    /// <summary>
    /// Clase singleton para guardar los datos de la Aplicacion
    /// </summary>
    public sealed class AppLogic
    {
        private LocationApiClient client = new LocationApiClient();
        private readonly static AppLogic _instance = new AppLogic();
        private ArrayList companies;
        private ArrayList entrepreneurs;

        private List<Rubro> validRubros = new List<Rubro>(){new Rubro("Alimentos"),new Rubro("Tecnologia"),new Rubro("Medicina")};

        private List<Qualifications> validQualifications = new List<Qualifications>(){new Qualifications("Vehiculo propio"),new Qualifications("Espacio para grandes volumenes de producto"),new Qualifications("Lugar habilitado para conservar desechos toxicos")};

        private List<Classification> validClasification = new List<Classification>(){new Classification("Organicos"),new Classification("Plasticos"),new Classification("Alimentos"),new Classification("Toxicos")};

        /// <summary>
        /// Obtiene las companias que estan registradas
        /// </summary>
        /// <value>companies</value>
        public ArrayList Companies
        {
            get{return new ArrayList(companies);}
        }

        /// <summary>
        /// Obtiene los emprendedores que estan registrados
        /// </summary>
        /// <value>entrepreneurs</value>
        public ArrayList Entrepreneurs
        {
            get{ return new ArrayList(entrepreneurs);}
        }

        /// <summary>
        /// Obtiene los rubros habilitados
        /// </summary>
        /// <value></value>
        public List<Rubro> Rubros
        {
            get{return validRubros;}
        }

        /// <summary>
        /// Obtiene la lista de habilitciones registradas
        /// </summary>
        /// <value></value>
        public List<Qualifications> Qualifications
        {
            get{return validQualifications;}
        }

        /// <summary>
        /// Obtiene la lista de clasificaciones/categorias registradas para los productos
        /// </summary>
        /// <value></value>
        public List<Classification> Classifications
        {
            get{ return validClasification;}
        }
        private AppLogic()
        {
            companies = new ArrayList();
            entrepreneurs = new ArrayList();
        }

        /// <summary>
        /// Obtiene la instancia de AppLogic
        /// </summary>
        /// <value></value>
        public static AppLogic Instance
        {
            get
            {
                return _instance;
            }
        }
        
        /// <summary>
        /// Metodo que registra a un emprendedor
        /// </summary>
        public void RegisterEntrepreneurs(string name, string ubication, Rubro rubro, List<Qualifications> habilitaciones,List<Qualifications> especializaciones)
        {
            entrepreneurs.Add(new Emprendedor(name,ubication,rubro,habilitaciones, especializaciones));
        }

        /// <summary>
        /// Metodo que retorna un mensaje con los rubros habilitaddos
        /// </summary>
        /// <returns></returns>
        public string ValidRubrosMessage()
        {
            StringBuilder message = new StringBuilder("Rubros habiliatdos:\n\n");
            int itemposition = 0;
            foreach (Rubro item in Rubros)
            {
                itemposition++;
                message.Append($"{itemposition}-"+item.RubroName+"\n"); 
            }
            return Convert.ToString(message);
        }

        /// <summary>
        /// Metdo que retorna un mensaje con las Habilitaciones permitidas
        /// </summary>
        /// <returns></returns>
        public string validQualificationsMessage()
        {
            StringBuilder message = new StringBuilder("Habilitaciones permitidas:\n\n");
            int itemposition = 0;
            foreach (Qualifications item in Qualifications)
            {
                itemposition++;
                message.Append($"{itemposition}-"+item.QualificationName+"\n"); 
            }
            return Convert.ToString(message);
        }

        /// <summary>
        /// Publica una oferta de la compania que se le ingresa
        /// </summary>
        /// <param name="company"></param>
        /// <param name="ifConstant"></param>
        /// <param name="tipo"></param>
        /// <param name="quantity"></param>
        /// <param name="cost"></param>
        /// <param name="ubication"></param>
        /// <param name="qualifications"></param>
        /// <param name="keyWords"></param>
        public void PublicOffer(Company company,bool ifConstant, Classification tipo, int quantity, double cost, string ubication, List<Qualifications> qualifications, ArrayList keyWords)
        {
            company.PublicOffer(ifConstant,tipo,quantity,cost,ubication,qualifications,keyWords);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por palabras clave
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public ArrayList SearchOfferByKeyWords(string word)
        {
            return OfferSearch.SearchByKeywords(word);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por tipo
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public ArrayList SearchOfferByType(string word)
        {
            return OfferSearch.SearchByType(word);
        }

        /// <summary>
        /// Metodo que se encarga de buscar las ofertas por ubicacion
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public ArrayList SearchOfferByUbication(string word)
        {
            return OfferSearch.SearchByUbication(word);
        }

        /// <summary>
        /// Metodo para aceptar una oferta
        /// </summary>
        /// <param name="emprendedor"></param>
        /// <param name="offer"></param>
        public string AccepOffer(Emprendedor emprendedor, Offer offer)
        {
            foreach(Qualifications item in offer.Qualifications)
            {
                if(!emprendedor.Qualifications.Contains(item))
                {
                    return "Usted no dispone de las habilitaciones requeridas por la oferta";
                }
            }
            offer.Buyer = emprendedor;
            offer.TimeAccepted = DateTime.Now;
            emprendedor.AddPurchasedOffer(offer);
            return "Usted a aceptado la oferta con exito";
        }

        /// <summary>
        /// Metodo que permite obtener la distancia entre un emprendedor y un producto
        /// </summary>
        public async Task<double> ObteinOfferDistance(Emprendedor emprendedor, Offer offer)
        {
            string emprendedorUbication = emprendedor.Ubication;
            string offerUbication = offer.Product.Ubication;
            Location locationEmprendedor = await client.GetLocation(emprendedorUbication);
            Location locationOffer = await client.GetLocation(offerUbication);
            Distance distance = await client.GetDistance(locationEmprendedor, locationOffer);
            double kilometers = distance.TravelDistance;
            await client.DownloadRoute(locationEmprendedor.Latitude, locationEmprendedor.Longitude,
            locationOffer.Latitude, locationOffer.Longitude, @"route.png");
            return kilometers;
        }

        /// <summary>
        /// Metodo que obtiene el mapa de la ubicacion de un emprendedor
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        public async Task ObteinOfferMap(Offer offer)
        {
            string offerUbication = offer.Product.Ubication;
            Location locationOffer = await client.GetLocation(offerUbication);
            await client.DownloadMap(locationOffer.Latitude, locationOffer.Longitude, @"map.png");
        }

        /// <summary>
        /// Metodo que devuelbe un string con la lista de materiales constantes
        /// </summary>
        /// <returns></returns>
        public (ArrayList, string) GetConstantMaterials()
        {
            Dictionary<Classification, int> clasificationDictionary = new Dictionary<Classification, int>();
            ArrayList constantMaterials = new ArrayList();
            foreach(Classification item in Classifications)
            {
                clasificationDictionary.Add(item,0);
            }
            foreach (Company company in Companies)
            {
                foreach (Offer offer in company.OffersPublished)
                {
                    if (offer.Constant)
                    {
                        constantMaterials.Add(offer.Product);
                        clasificationDictionary[offer.Product.Classification] += 1;
                    }
                }
            }
            StringBuilder message = new StringBuilder();
            message.Append("Los tipos de materiales mas constantes en nuestras ofertas son:\n\n");
            foreach(Classification item in Classifications)
            {
                message.Append($"{item.Category} con {clasificationDictionary[item]} ofertas\n");
            }
            return (constantMaterials,Convert.ToString(message));
        }

        /// <summary>
        /// Obtiene un string con la indicando si sus ofertas fueron o no fueron aceptadas, en caso de que si, indica ademas la fecha de cuando fueron aceptadas
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public string GetOffersAccepted(Company company)
        {
            StringBuilder message = new StringBuilder();
            foreach (Offer item in company.OffersPublished)
            {
                if (item.Buyer != null)
                {
                    message.Append($"{item.Product.Quantity} {item.Product.Classification} Accepted at {item.TimeAccepted}\n");
                }
                else 
                {
                    message.Append($"{item.Product.Quantity} of {item.Product.Classification} not Accepted");
                }
            }
            return Convert.ToString(message);
        }

        /// <summary>
        /// Obtiene las ofertas aceptadas por el emprendedor, junto con la fecha de cuando las acepto
        /// </summary>
        /// <param name="emprendedor"></param>
        /// <returns></returns>
        public string GetOffersAccepted(Emprendedor emprendedor)
        {
            StringBuilder message = new StringBuilder();
            foreach (Offer item in emprendedor.PurchasedOffers)
            {    
                message.Append($"{item.Product.Quantity} {item.Product.Classification} at a price of {item.Product.Price}$ Accepted at {item.TimeAccepted}\n");
            }
            return Convert.ToString(message);
        }

        /// <summary>
        /// Obtiene la cantidad de ofertas que furon aceptadas en un periodo de tiempo establecido por el usuario
        /// </summary>
        /// <param name="company"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public int GetPeriodTimeOffersAccepted(Company company, int periodTime)
        {
            int offersAccepted = 0;
            foreach(Offer offer in company.OffersPublished)
            {
                if (offer.Buyer != null)
                {
                    int diference = Convert.ToInt32(offer.TimeAccepted - DateTime.Now);
                    if(diference <= periodTime)
                    {
                        offersAccepted += 1;
                    }
                }
            }
            return offersAccepted;
        }

        /// <summary>
        /// Obtiene la cantidad de ofertas que furon aceptadas en un periodo de tiempo establecido por el usuario
        /// </summary>
        /// <param name="emprendedor"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public int GetPeriodTimeOffersAccepted(Emprendedor emprendedor, int periodTime)
        {
            int offersAccepted = 0;
            foreach(Offer offer in emprendedor.PurchasedOffers)
            {
                if (offer.Buyer != null)
                {
                    int diference = Convert.ToInt32(offer.TimeAccepted - DateTime.Now);
                    if(diference <= periodTime)
                    {
                        offersAccepted += 1;
                    }
                }
            }
            return offersAccepted;
        }
    }

}
