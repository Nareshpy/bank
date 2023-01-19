using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    internal class CurrencyConverter
    {
        public static Dictionary<string, double> all_currencies = new Dictionary<string, double>()
        {
            { "Argentine Peso" ,  0.448857 },{ "Australian Dollar" ,   56.552696 },{ "Bahraini Dinar" ,  216.045710 },
            {"Botswana Pula"   ,  6.374151 },{"Brazilian Real" , 15.902221 },{"British Pound"  , 99.153185 },
            {"Bruneian Dollar" , 61.418621 },{"Bulgarian Lev"   , 45.053369 },{"Canadian Dollar" ,  60.716094 },
            {"Chilean Peso"    ,  0.099238 },{"Chinese Yuan Renminbi"  ,    12.075479 },{"Colombian Peso"  ,  0.017328 },
            {"Czech Koruna"   ,   3.665050 },{"Danish Krone"    , 11.845787 },{"Emirati Dirham"  ,  22.119316 },
            {"Euro"    ,   88.116731 },{"Hong Kong Dollar"   ,   10.400372 },{"Hungarian Forint"   ,  0.221696 },
            {"Icelandic Krona" ,  0.571027 },{"Indonesian Rupiah"  ,  0.005360 },{"Iranian Rial" ,  0.001935 },
            {"Israeli Shekel"  ,  23.754456 },{"Japanese Yen"    ,   0.629103 },{"Kazakhstani Tenge"  ,   0.175769 },
            {"Kuwaiti Dinar"   ,    265.995443 },{"Libyan Dinar"   ,  17.113893 },{"Malaysian Ringgit" ,  18.731828 },
            {"Mauritian Rupee" , 1.824934 },{"Mexican Peso" , 4.309346 },{"Nepalese Rupee"  ,   0.624707 },
            {"New Zealand Dollar" ,51.849767 },{"Norwegian Krone" ,  8.199889 },{"Omani Rial"  , 210.988973 },
            {"Pakistani Rupee" ,  0.355252 },{"Philippine Peso" ,  1.474880 },{"Polish Zloty" ,  18.762034 },
            {"Qatari Riyal"  ,  22.316810 },{"Romanian New Leu "  , 17.826594 },{"Russian Ruble"   ,    1.198431 },
            {"Saudi Arabian Riyal" ,   21.662183 },{"Singapore Dollar"   ,  61.418621 },{"South African Rand" ,  4.853667 },
            {"South Korean Won" , 0.065426 },{"Sri Lankan Rupee" , 0.221045 },{"Swedish Krona"  ,   7.791271 },
            {"Swiss Franc" ,    87.475837 },{"Taiwan New Dollar" , 2.674150 },{"Thai Baht"  , 2.450504},
            {"Trinidadian Dollar"  ,  11.973762 },{"Turkish Lira" ,  4.324127 },{"US Dollar" , 81.233187 },
            {"Venezuelan Bolivar" ,   0.000043}
        };
        public static double ShowCurrencies()
        {
            Console.WriteLine("Please enter the required currency as exactly shown in the options");
            Console.WriteLine("\t1.Argentine Peso Australian Dollar\t2.Bahraini Dinar\t3.Botswana Pula\n\t4.Brazilian Real\t5.British Pound\t6.Bruneian Dollar\n\t7.Bulgarian Lev\t8.Canadian Dollar\t9.Chilean Peso\n\t10.Chinese Yuan Renminbi\t11.Colombian Peso\t12.Czech Koruna\n\t13.Danish Krone\t14.Emirati Dirham\t15.Euro\n\t16.Hong Kong Dollar\t17.Hungarian Forint\t18.Icelandic Krona\n\t19.Indonesian Rupiah\t20.Iranian Rial\t21.Israeli Shekel\n\t22.Japanese Yen\t23.Kazakhstani Tenge\t24.Kuwaiti Dinar\n\t25.Libyan Dinar\t26.Malaysian Ringgit\t27.Mauritian Rupee\n\t28.Mexican Peso\t29.Nepalese Rupee\t30.New Zealand Dollar\n\t31.Norwegian Krone\t32.Omani Rial\t33.Pakistani Rupee\n\t34.Philippine Peso\t35.Polish Zloty\t36.Qatari Riyal\n\t37.Romanian New Leu\t38.Russian Ruble\t39.Saudi Arabian Riyal\n\t40.Singapore Dollar\t41.South African Rand\t42.South Korean Won\n\t43.Sri Lankan Rupee\t44.Swedish Krona\t45.Swiss Franc\n\t46.Taiwan New Dollar\t47.Thai Baht\t48.Trinidadian Dollar\n\t49.Turkish Lira\t50.US Dollar\t51Venezuelan Bolivar\t");
            string selected_currency=Console.ReadLine();
            return all_currencies[selected_currency];
        }
    }

}
