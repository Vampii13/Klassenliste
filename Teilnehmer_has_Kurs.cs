//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Klassenliste
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teilnehmer_has_Kurs
    {
        public int IdTeilnehmerKurs { get; set; }
        public int IdTeilnehmer { get; set; }
        public int IdKurs { get; set; }
    
        public virtual Kurs Kurs { get; set; }
        public virtual Teilnehmer Teilnehmer { get; set; }
    }
}
