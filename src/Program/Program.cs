﻿//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyect;

namespace ConsoleApplication
{
    /// <summary>
    /// Programa de consola de demostración.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main.
        /// </summary>
        /// <returns>Task.</returns>
        public static async Task Main()
        {
            AppLogic.Instance.RegisterEntrepreneurs("ABD","Matias", "Palacio Legislativo", AppLogic.Instance.Rubros[0], AppLogic.Instance.Qualifications, new ArrayList(){"Desechos organicos"});
            AppLogic.Instance.RegisterEntrepreneurs("ACD","Matias", "Cordoba", AppLogic.Instance.Rubros[1], new List<Qualifications>(){AppLogic.Instance.Qualifications[0], AppLogic.Instance.Qualifications[1]}, new ArrayList(){"Desechos plasticos"});
            Company c1 = new Company("GFYT","MatiasCorp", "Parque Rodó", AppLogic.Instance.Rubros[1]);
            AppLogic.Instance.Companies.Add(c1);
            AppLogic.Instance.PublicConstantOffer(c1, AppLogic.Instance.Classifications[3], 300, 5000, "Parque Rodó", AppLogic.Instance.Qualifications, new ArrayList(){"Toxicos","Grandes volumenes"});
            AppLogic.Instance.AccepOffer(AppLogic.Instance.Entrepreneurs[0], c1.OffersPublished[0]);
            await AppLogic.Instance.ObteinOfferMap(c1.OffersPublished[0]).ConfigureAwait(true);
            Console.WriteLine(await AppLogic.Instance.ObteinOfferDistance(AppLogic.Instance.Entrepreneurs[0], c1.OffersPublished[0]).ConfigureAwait(true) + " Kilometers");
            Console.WriteLine(AppLogic.Instance.ValidRubrosMessage());
            Console.WriteLine(AppLogic.Instance.validQualificationsMessage());
            Console.WriteLine(AppLogic.Instance.GetConstantMaterials());
            Console.WriteLine(AppLogic.Instance.GetOffersAccepted(c1));
            Console.WriteLine(AppLogic.Instance.GetOffersAccepted(AppLogic.Instance.Entrepreneurs[0]));
            Console.WriteLine(AppLogic.Instance.SearchOfferByType("Toxicos")[0]);
        }
    }
}