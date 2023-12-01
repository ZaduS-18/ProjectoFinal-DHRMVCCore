using System;
using System.Collections.Generic;

namespace DHRMVCCore.Models;

public partial class FechaCierre
{
    public int IdFCierre { get; set; }

    public int TipPago { get; set; }

    public DateTime FRecepcion { get; set; }

    public DateTime FCierre { get; set; }
}
