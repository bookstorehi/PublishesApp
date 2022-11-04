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
    
    public partial class Отправления
    {
        public int ИД { get; set; }
        public int Номер_подписки { get; set; }
        public string Имя_получателя { get; set; }
        public string Фамилия_получателя { get; set; }
        public string Отчество_получателя { get; set; }
        public string Должность { get; set; }
        public System.DateTime Предполагаемая_дата { get; set; }
        public Nullable<System.DateTime> Дата_получения { get; set; }
        public string Организация
        {
            get { return this.Подписки.Организация; }
        }
        public string Издание
        {
            get { return this.Подписки.Издания.Названия; }
        }
        public string Получатель
        {
            get
            {
                if (this.Отчество_получателя != null)
                    return this.Фамилия_получателя + " " + this.Имя_получателя[0] + ". " + this.Отчество_получателя[0] + ".";
                else
                    return this.Имя_получателя + " " + this.Фамилия_получателя;
            }
        }
        public string Примерная_дата
        {
            get {  return Подписки.ФорматированнаяДата(this.Предполагаемая_дата); }
        }
        public string Дата
        {
            get
            {
                if (this.Дата_получения != null)
                    return Подписки.ФорматированнаяДата((DateTime)this.Дата_получения);
                else
                    return "Не получено";
            }
        }
        public string Доставка
        {
            get { return this.Подписки.Способ_доставки; }
        }
    
        public virtual Подписки Подписки { get; set; }
    }
}
