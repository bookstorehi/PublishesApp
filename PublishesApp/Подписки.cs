//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PublishesApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Подписки
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Подписки()
        {
            this.Отправления = new HashSet<Отправления>();
        }
    
        public int ИД { get; set; }
        public string Организация { get; set; }
        public System.DateTime Дата_начала { get; set; }
        public System.DateTime Дата_окончания { get; set; }
        public decimal Стоимость { get; set; }
        public int Периодичность_выхода { get; set; }
        public string Способ_доставки { get; set; }
        public string Индекс_издания { get; set; }
        public string Начало
        {
            get { return ФорматированнаяДата(this.Дата_начала); }
        }
        public string Конец
        {
            get { return ФорматированнаяДата(this.Дата_окончания); }
        }
        public string Сумма
        {
            get
            {
                return (int)this.Стоимость + " р.";
            }
        }
        public string Период
        {
            get
            {
                return (int)this.Периодичность_выхода + " дней";
            }
        }
        public string Формат1
        {
            get
            {
                return this.Организация + " - " + this.Издания.Названия;
            }
        }
        public virtual Издания Издания { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Отправления> Отправления { get; set; }
        public string ФорматированнаяДата(DateTime d)
        {
            string day;
            string month;

            if (d.Day > 9)
                day = d.Day.ToString();
            else
                day = "0" + d.Day.ToString();

            if (d.Month > 9)
                month = d.Month.ToString();
            else
                month = "0" + d.Month.ToString();

            return day + "." + month + "." + d.Year.ToString().Substring(2);
        }
    }
}
