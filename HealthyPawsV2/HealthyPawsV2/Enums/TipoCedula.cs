namespace HealthyPawsV2.Enums
{
    using System.ComponentModel;
    public enum TipoCedula
    {
        [Description("Nacional")]
        NACIONAL,

        [Description("Extranjero")]
        EXTRANJERO,

        [Description("Nacionalizado")]
        NACIONALIZADO,
    }
}
