using PlaginIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project1._1
{
	public partial class Form1 : Form, IPluginHost
	{
		public Form1()
		{
			InitializeComponent();
		}

		List<IPlugin> _plugins;


		private void LoadPlugins(string path)
		{
			string[] pluginFiles = Directory.GetFiles(path, "*.dll");
			this._plugins = new List<IPlugin>();

			foreach (string pluginPath in pluginFiles)
			{
				Type objType = null;
				try
				{
					// пытаемся загрузить библиотеку

					Assembly assembly = Assembly.LoadFrom(pluginPath);


					if (assembly != null)
					{

						{
							var types = assembly.GetTypes().
						Where(t => t.GetInterfaces().
						Where(i => i.FullName == typeof(IPlugin).FullName).Any());

							foreach (var type in types)
							{
								var plugin = assembly.CreateInstance(type.FullName) as IPlugin;

								_plugins.Add(plugin);
							}

						}

					}
				}
				catch
				{
					continue;
				}

			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

			this.LoadPlugins(Path.Combine(Application.StartupPath, "Plugins"));
			this.AddPluginsItems();
		}

		//	public List<Cell> listToken { get; set; }

		public void button1_Click(object sender, EventArgs e)
		{
			string otvet;
			List<Cell> listToken = new List<Cell>();
			expression = textBox1.Text;
			string exeptionParsing = "";
			bool preprocesstry = preprocess(expression += '!');
			if (preprocesstry)
			{ 
				listToken = Parsing.process(expression);
				foreach (var token in listToken)
				{
					exeptionParsing += token.Token;
				}
				if (_plugins.Count == 0)
				{
					MessageBox.Show("Такой плагин отсутствует");
					expression = expression.Remove(expression.Length - 1, 1);
					textBox1.Text = exeptionParsing; // полученное выражение
				}
				else
				{
					foreach (var plug in _plugins)
					{
						try
						{
							otvet = plug.activate(exeptionParsing);


							if (otvet == exeptionParsing)
							{
								textBox1.Text = otvet;
								MessageBox.Show("Такой плагин отсутствует");
								break;
							}
							else
							{
								textBox1.Text = exeptionParsing;
								MessageBox.Show(otvet);
							}
						}


						catch
						{
							MessageBox.Show("Что-то пошло не так");
						}
					}
				}
			}
			else
			{
				expression = expression.Remove(expression.Length-1, 1);
				textBox1.Text = expression;
			}
			
				
			
		}

		bool preprocess(string data)
		{
			if (string.IsNullOrEmpty(data))  //строка не будет null
			{
				MessageBox.Show("Строка пустая, введите данные");
				return false;
			}
			
			string equaally = "";
			int parentheses = 0;
			int leter = 0;
			int digit = 0;
			int index = 0;
			try
			{
				for (int i = 0; i < data.Length; i++)
			{
				
					index = i;
					char ch = data[i];
					if (ch == '!' || ch == '?' || ch == '.' || ch == ',' || ch == '`' || ch == '~' || ch == '@' || ch == '#' || ch == '%' || ch == '&' ||
						ch == '<' || ch == '>')
					{
						MessageBox.Show("Введен неправильный символ");
						return false;
					}
					if (ch != '!')
					{
						if (Char.IsDigit(ch))
						{
							digit++;
						}
						if (Char.IsLetter(ch))
						{
							leter++;
						}
						if (Char.IsDigit(ch) && Char.IsLetter(data[++index]))
						{
							MessageBox.Show("Не верное значение");
							return false;
						}
						if (Char.IsLetter(ch) && Char.IsDigit(data[++index]))
						{
							MessageBox.Show("Не верное значение");
							return false;
						}
						if (getOperation(ch) && getOperation(data[++index]))
						{
							MessageBox.Show("Не верное значение");
							return false;
						}
						switch (ch)
						{
							case '=':
								{
									while (ch != '!')
									{
										ch = data[++i];
										equaally += data[i];
									}
								}
								break;
							case ' ':
								{
									MessageBox.Show("Введены пробелы");
									return false;
								}
							case '\n':
							case '!': continue;
							case Parsing.END_ARG:
								parentheses--;
								break;
							case Parsing.START_ARG:
								parentheses++;
								break;
						}
					}
				}
			    if (parentheses != 0)
				{
					MessageBox.Show("Разное количество скобок");
					return false;
				}
				if (!getDigit(equaally))
				{
					MessageBox.Show("Значение после '=' неопределено");
					return false;
				}
				if (equaally == "")
				{
					MessageBox.Show("Не введено '='");
					return false;
				}
				if (leter == 0)
				{
					MessageBox.Show("Не введена неизвестная");
					return false;
				}
				if (digit == 0)
				{
					MessageBox.Show("Не введены числовые переменные");
					return false;
				}
			}
			catch
			{
				MessageBox.Show("Что-то пошло не так. Оставайтесь с нами");
			}
			
			return true;
		}

		static bool getDigit(string data)
		{
			data = data.Remove(data.Length - 1, 1);
			foreach (var ch in data)
			{
				if (!Char.IsDigit(ch))
				{
					return false;
				}
			}
			return true;
		}

		static bool getOperation(char ch) //возвращает матем оператор
		{
			return ch == '*' || ch == '/' || ch == '+' || ch == '-' || ch == '^' || ch == '=' || ch == '=';
		}

		public bool Register(IPlugin plug)
		{
			throw new NotImplementedException();
		}

		static string expression { get; set; }




		private void AddPluginsItems()
		{


			foreach (IPlugin plugin in this._plugins)
			{
				if (plugin == null)
				{
					continue;
				}

			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("Title of this help project.chm");
		}
	}
}
