using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace AutoCAD_ASPZ {
    public partial class Form_ASPZ_Pj : Form {

        #region Блок переменных
        // Список примитивов (блоков) с чертежа
        readonly List<DeviceClass> entityList = new List<DeviceClass>();
        // Класс сериализованных устройств (примитивы собираются в устройство)
        Device device;
        List<Device> devices = new List<Device>();
        // Словарь слияния примитивой чертежа и файла xml,  Dictionary<TKey,TValue> класс предоставляет сопоставление
        // из набора ключей с набором значений.
        // Каждый элемент, добавляемый в словарь, состоит из значения и связанного с ним ключа.
        Dictionary<string, DeviceReport> dicDeviceReport = new Dictionary<string, DeviceReport>();

        // Временные переменные
        string brHandle = "";
        string brName = "";
        string arHandle = "";
        string arTextString = "";

        #endregion

        public Form_ASPZ_Pj() {
            InitializeComponent();
        }

        private void Form_ASPZ_Pj_Load(object sender, EventArgs e) {

            //EventArgs - это класс, дающий возможность передать какую-нибудь дополнительную информацию обработчику
            //(например, текущие координаты мыши при событии MouseMove).
            //sender - это объект, который вызвал событие.

            //Чтение (создание) файла xml типа устройств (читаем устройства по одному и добавляем в список дивайсов)
            SerialtMethod();
            // Инициализация словаря по файлу xml
            InitDeviceClassMethod();
            // Обновление визуализации рапорта
           // this.reportViewer1.RefreshReport();

        }

        /// <summary>
        /// Инициализация словаря по файлу xml
        /// </summary>
        private void InitDeviceClassMethod() {
            foreach (var item in devices) {
                //dicDeviceReport.Add(item.DeviceType, new DeviceReport { TypeDescription = item.TypeDescription, TypeCost = 0, TypeCount = 0} );
                dicDeviceReport.Add(item.DeviceType, new DeviceReport(item.TypeDescription, 0, 0));
            }
        }

        /// <summary>
        /// Чтение (создание) файла xml типа устройств
        /// </summary>
        private void SerialtMethod() {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Device>));
            //Сериализация - это операция преобразования объекта в формат, пригодный для записи на диск или отправки в сеть.
            //Десериализация - это операция восстановления объекта из сериализованного представления.

            // Создание xml файла
            void WrFl() {
                device = new Device("ППКУП", "Прибор приемно-контрольный и управления пожарный блочно-модульный рудничный особовзрывобезопасный");
                devices.Add(device);
                device = new Device("ОПСЗ-2-485", "Оповещатель пожарный комбинированный светозвуковой рудничный особовзрывобезопасный");
                devices.Add(device);
                device = new Device("ИП 101-2-PR-485", "Извещатель пожарный тепловой рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("ИП 513-2-В-485", "Извещатель пожарный ручной рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("УДП 513-2-В-485", "Устройство дистанционного пуска рудничное особовзрывобезопасное ");
                devices.Add(device);
                device = new Device("ИП 329/330-1-1-485", "Извещатель пожарный пламени рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("RM 485-802.15.4-1T4L-***", "Радиомодуль 802.15.4 ");
                devices.Add(device);
                device = new Device("ИП 101-3-PR", "Извещатель пожарный тепловой радиоканальный рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("ИП 513-1-В", "Извещатель пожарный ручной радиоканальный рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("УДП 513-1-В", "Устройство дистанционного пуска радиоканальное рудничное особовзрывобезопасное ");
                devices.Add(device);
                device = new Device("ИП 329/330-1-1", "Извещатель пожарный пламени радиоканальный рудничный особовзрывобезопасный ");
                devices.Add(device);
                device = new Device("БУ-1", "Блок управления");
                devices.Add(device);

                using (FileStream fileStream = new FileStream("device.xml", FileMode.OpenOrCreate)) {
                    xmlSerializer.Serialize(fileStream, devices);
                }

            }
            // Чтение xml файла
            void RrFl() {
                devices.Clear();
                if (!File.Exists("device.xml")) {
                    MessageBox.Show("Десериализация невозможна. \n Отсутствует файл данных.");
                    return;
                }
                // Десериализация
                using (FileStream fileStream = new FileStream("device.xml", FileMode.Open)) {
                    devices = (List<Device>)xmlSerializer.Deserialize(fileStream);
                }
            }

            WrFl(); 
            RrFl();
        }

        /// <summary>
        /// Отображение списка блоков dll myCommands (AutoCAD)
        /// </summary>
        /// <param name="sTr"></param>
        public void ADD_richTextBox(string sTr) {
            richTextBox1.AppendText(sTr + "\n");
        }

        /// <summary>
        /// Формирование списка блоков dll myCommands (AutoCAD)
        /// </summary>
        /// <param name="d"></param>
        public void ADD_entityList(DeviceClass d) {
            entityList.Add(d);
        }

        //string brHandle = "";
        //string brName = "";
        //string arHandle = "";
        //string arTextString = "";
        public void ADD_entityList(string[] arr) {
            //string.Equals() - Определяет, равны ли значения двух объектов String.
            if (string.Equals(arr[0], brHandle)) {
                // Если равны (Блок повторяется, добавляются атрибуты к уже существующему блоку) -
                // - в список слоков добавляется только атрибут
                // entityList - список блоков (entityList.Count-1 - последний элемент списка)
                entityList[entityList.Count-1].Attribute.Add(arr[2], arr[3]);
            }
            else {
                // Блок имеет первое вхождение
                brHandle = arr[0];
                //создаем новый блок dC ,добавляем вхождение
                DeviceClass dC = new DeviceClass {
                    BR_Handle = arr[0],
                    BR_Name = arr[1]
                };
                //добавляем атрибуты
                dC.Attribute.Add(arr[2], arr[3]);
                //добавляем в список блоков
                entityList.Add(dC);
            }
        }

        //public void NAP_Scan()
        //{

        //}


        /// <summary>
        /// Окончание прохода по чертежу
        /// </summary>
        public void END_Scan() {
            //вызывается в конце myCommands (пеерчисляем все элементы в списке блоков)
            foreach (var item in entityList) {
                if (dicDeviceReport.ContainsKey(item.BR_Name)) {
                    // Подсчет количества элементов типа
                    dicDeviceReport[item.BR_Name].TypeCount++;
                }
                else {
                    // Поиск альтернативных имен (особенности написания-именования блоков)
                    switch (item.BR_Name) {
                        case "RM 485-802.15.4-1T4L":
                            dicDeviceReport["RM 485-802.15.4-1T4L-***"].TypeCount++;
                            break;
                        case "ИП 329-330-1-1-485":
                            dicDeviceReport["ИП 329/330-1-1-485"].TypeCount++;
                            break;
                        case "ИП 329-330-1-1":
                            dicDeviceReport["ИП 329/330-1-1"].TypeCount++;
                            break;                      
                    }
                }
            }

            //
            DataSetSpecification.DS_RepDataTable dS_Reps = new DataSetSpecification.DS_RepDataTable();
            DataSetSpecification.DS_RepRow dS_RepRow;// = dS_Reps.NewDS_RepRow();

            int a = 0;

            foreach (var item in dicDeviceReport.Where(za => za.Value.TypeCount > 0)) {
                dS_RepRow = dS_Reps.NewDS_RepRow();
                                
                dS_RepRow["position"] = ++a;
                dS_RepRow["nameOfEquipment"] = item.Value.TypeDescription;
                dS_RepRow["typeOfEquipment"] = item.Key;
                dS_RepRow["units"] = "шт";
                dS_RepRow["inAll"] = item.Value.TypeCount;

                dS_Reps.Rows.Add(dS_RepRow);
            }

          //  reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rd1 = new ReportDataSource("DataSet1", dS_Reps.ToList());
          //  reportViewer1.LocalReport.DataSources.Add(rd1);

         //   this.reportViewer1.RefreshReport();
        }



    }

    /// <summary>
    /// Класс для взаимодействия с dll myCommands (AutoCAD)
    /// </summary>
    public class DeviceClass {
        //BlockTableRecord 
        //public int BTR_Handle { get; set; }

        //BlockReference
        public string BR_Handle { get; set; }
        public string BR_Name { get; set; }

        //AttributeReference
        public string AR_Handle { get; set; }
        public string AR_Atribut { get; set; }

        public Dictionary<string, string> Attribute = new Dictionary<string, string>();
    }

    //подключение датчиков к NAP(коммутатору-ретранслятору), (port_nap)
    //количество зон, контролируемых данным NAP, 
    //количество устройств в пределах зоны,
    //максимальный адрес на полевой шине, распределение подключенных датчиков по портам полевой шины, (address)
    //наличие всех необходимых технических средств для полной комплектации системы(датчиков, блоков питания), (n - считать колличество)
    //учет резервирования необходимых элементов, (n)
    //потребляемые схемой напряжение и мощность, (voltage - суммарная 127, power)
    //полученная общая стоимость датчиков, используемых при проектировании (cost)

    /// <summary>
    ///ППКУП
    /// </summary>

    public class DevicePPKUP //NAP port_4
    {
        public int n; //количество ППКУП
        public double cost;
        public double voltage; //рассчетная согласно исполнению
        public int n_address; //число адресов на один ШПС
        public int address_max = 255; //максимальное число адресов на один ШПС 
        public int zone;

        public DevicePPKUP()
        {
            n = 0;
            n_address = 0;
        }

    }

    /// <summary>
    ///проводные
    ///Проводные устройства в качестве источника питания используют ИИП-М-2.0-2.0-Ех 
    ///через специализированное присоединение в шкафе ППКУП
    /// </summary>

    public class DeviceOPSZ //ОПСЗ-2-485 //NAP port_2
    {
        //Оповещатель пожарный комбинированный светозвуковой рудничный 
        //особовзрывобезопасный - техническое средство, предназначенное для оповещения людей
        //о пожаре посредством подачи светового и/или звукового сигнала с целью воздействия на
        //органы чувств человека.
        public int n;
        public int n_max;

        public int nap;
        public int zone;
        public int port_nap;
        public int address;

        public double cost;
        public double voltage = 12;
        public double power1 = 0.06; //в дежурном режиме
        public double power2 = 1.5; //в режиме оповещения
        
        public DeviceOPSZ()
        {
            n = 0;
        }
    }
    public class DeviceUDP_wired //УДП-513-2-В-485 //NAP port_2
    {
        //должны быть привязаны к защищаемой зоне
        public int n;

        public double cost;
        public double voltage = 12;
        public double power = 0.1;


        public int nap;
        public int zone;
        public int port_nap;
        public int address;
        public DeviceUDP_wired()
        {
            n = 0;
        }
    }
    public class DeviceIP_wired //ИП 101-2-PR-485, ИП 513-2-В-485,  ИП 239/330-1-1-485 //NAP port_2
    {
        //С точки зрения ПО обязательным являются задание номера зоны 
        //контроля пожарной сигнализации и адреса на шине RS-485
        public int n;

        public double cost;
        public double voltage;
        public double power;

        public int nap;
        public int zone;
        public int port_nap;
        public int address;
        public DeviceIP_wired(double voltage, double power)
        {
            n = 0;
          //  this.cost = cost;
            this.voltage = voltage;
            this.power = power;
        }
    }

    /// <summary>
    ///радиомост РМ 485-802.15.4 -1T2L-***
    ///является логическим связующим элементом между беспроводными ТСПА и шкафом ППКУП
    ///не предназначен для принятия самостоятельных решений
    ///RM 485-802.15.4-1T4L-*** подлежит обязательной конфигурации для задания адреса на шине RS485 и логической зоны привязки
    /// </summary>

    public class DeviceRM //NAP port_3
    {
        public int n;
        
        //public int n_max = 55; //максимальное количество опрашиваемых устройств
        public double lenght = 300;  //максимальная дальность сввязи 300 м
        public double voltage = 12;
        public double power = 0.1;
        public double cost;

        public int nap;
        public int zone;
        public int port_nap;
        public int address;
        public DeviceRM()
        {
            n = 0;
        }
    }

    /// <summary>
    ///беспроводные к РМ 485-802.15.4 -1T2L-***
    ///резервируемое автономное батарейное питание и тип канала связи
    ///Беспроводные ТСПА на уровне кода привязываются к идентификатору NAP100.D-3D4R4T2F2X и рабочей зоне
    /// </summary>

    public class DeviceIP_wireless //ИП 101-3-PR, ИП 513-1-В, ИП 239/330-1-1
    {
        //С точки зрения ПО обязательным являются задание номера зоны 
        //контроля пожарной сигнализации и адреса на шине RS-485
        public int n;

        public double cost;        
        public double lenght;

        public int zone;
        public int nap;
        public DeviceIP_wireless(double lenght)
        {
            n = 0;
           // this.cost = cost;
            this.lenght = lenght;
        }
    }

    public class DeviceUDP_wireless //УДП-513-1-В
    {
        public int n;
        public double cost;

        public int zone;
        public int nap;
        public DeviceUDP_wireless()
        {
            n = 0;
           // this.cost = cost;
        }
    }
    
    public class DeviceBU //БУ-1
    {
        //Самостоятельного назначения не имеет. Совместно с модулями
        //пожаротушения(МПП, МУПТВ) применяется для контроля их состояния и исполнения сигналов от АСПЗ
        //комплектуется извещателем пожарным тепловым рудничным особовзрывобезопасным ИП 101-4-PR
        public int n;
        public int n_max;
        public double cost;

        public int zone;
        public int nap;
        public DeviceBU()
        {
            n = 0;
          //  this.cost = cost;
        }
    }
    


    /// <summary>
    /// Класс описания типов устройств АСПЗ
    /// </summary>
    [Serializable]
    public class Device {
        // Тип устройства
        public string DeviceType { get; set; }
        // Описание типа
        public string TypeDescription { get; set; }

        public Device() { }

        public Device(string s1, string s2) {
            DeviceType = s1;
            TypeDescription = s2;
        }
    }

    /// <summary>
    /// Класс сливания файла xml и элементов чертежа
    /// </summary>
    public class DeviceReport {
        // Описание типа
        public string TypeDescription;
        // Количество устройств типа
        public int TypeCount;
        // Стоимость устройств типа
        public int TypeCost;

        public DeviceReport() {
            TypeDescription = "";
            TypeCount = 0;
            TypeCost = 0;
        }

        public DeviceReport(string s1, int i1, int i2) {
            TypeDescription = s1;
            TypeCount = i1;
            TypeCost = i2;
        }


    }
}
