using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace model
{
    public class dependencia
    {
        public int DEPE_CODI { get; set; }
        public string DEPE_NOMB { get; set; }
        public int DPTO_CODI { get; set; }
        public int DEPE_CODI_PADRE { get; set; }
        public int MUNI_CODI { get; set; }
        public int DEPE_CODI_TERRITORIAL { get; set; }
        public string DEP_SIGLA { get; set; }
        public int DEP_CENTRAL { get; set; }
        public string DEP_DIRECCION { get; set; }
        public int DEPE_NUM_INTERNA { get; set; }
        public int DEPE_NUM_RESOLUCION { get; set; }
        public int DEPE_RAD_TP1 { get; set; }
        public int DEPE_RAD_TP2 { get; set; }
        public int ID_CONT { get; set; }
        public int ID_PAIS { get; set; }
        public int DEPE_ESTADO { get; set; }
        public int DEPE_RAD_TP3 { get; set; }
        public int DEPE_RAD_TP4 { get; set; }
        public int DEPE_NUEVA { get; set; }
        public int DEPE_RAD_TP5 { get; set; }
        public string DEPE_CARGO { get; set; }
        public int DEPE_RAD_TP6 { get; set; }
        public int DEPE_NIVEL { get; set; }

    }
}
