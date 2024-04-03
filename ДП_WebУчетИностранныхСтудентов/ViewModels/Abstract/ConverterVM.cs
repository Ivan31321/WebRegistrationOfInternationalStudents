namespace MonitoringTheProgressOfForeignStudents.ViewModels.Abstract
{
    public abstract class ConverterVM<TFrom, TTo>
    {
        public abstract TTo ConvertTo(TFrom obj);
        public abstract TFrom ConvertFrom(TTo obj);
        public abstract TTo Update(TFrom from, TTo to);
    }
}
