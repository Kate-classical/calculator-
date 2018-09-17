using PlaginIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin2
{
	public class Class1 : IPlugin
	{
		public bool GetBool { get; set; }
				
		public string activate(string input)
		{
			string a = "";
			string b = "";
			string c = "";
			List<Cell> listToken = BreakToken(input);
			if (GetYes(listToken,ref a, ref b, ref c))
			{
				if (proverka(ref a,ref b,ref c))
				{
					if (a == "") a = "1";
					if (b == "") b = "1";
					if (c == "") c = "1";
					GetBool = true;
					int aa = Convert.ToInt32(a);
					int bb = Convert.ToInt32(b);
					int cc = Convert.ToInt32(c);
					double d = bb * bb - 4 * aa * cc;
					if (d < 0)
					{
						return "Меньше нуля";
					}
					if (d == 0)
					{
						double x = (-bb / (2 * aa));
						return "Корень равен " + x.ToString();
					}
					if (d > 0)
					{
						double x1 = ((-bb - Math.Sqrt(bb)) / (2 * aa));
						x1 = Math.Round(x1, 3);
						double x2 = ((-bb + Math.Sqrt(bb)) / (2 * aa));
						if (x2 < 0)
						{
							x2 *= -1;
						}
						x2 = Math.Round(x2, 3);
						return "Корни равны: " + x1.ToString() + " и " + x2.ToString();
					}
				}
				
				if (!Char.IsDigit(a[0]) && !Char.IsDigit(b[0]) && !Char.IsDigit(c[0]))
				{

				}

				return "!!!!";
			}
			
			return input;
		}

		public bool proverka(ref string a,ref string b,ref string c)
		{
			if (a == "")
			{
				a = "1";
			}
			else
			{
				if (a[0] == '*' || a[0] == '/' || a[0] == '^' )
				{
					return false;
				}
				else
				{
					if (a[0] == '+' || a[0] =='-')
					{
						a = a.Remove(0, 1);
					}
					else
					{
						if (Char.IsDigit(a[0]))
						{
							return true;
						}
					}
				}

			}
			if (!Char.IsDigit(a.Last()))
			{
				a = a.Remove(a.Length- 1, 1);
			}
			if (b == "")
			{
				b = "1";
			}
			else
			{
				if (b[0] == '*' || b[0] == '/' || b[0] == '^')
				{
					return false;
				}
				else
				{
					if (b[0] == '+' || b[0] == '-')
					{
						b = b.Remove(0, 1);
					}
					else
					{
						if (Char.IsDigit(b[0]))
						{
							return true;
						}
					}
				}

			}

			if (!Char.IsDigit(b.Last()))
			{
				b = b.Remove(b.Length - 1, 1);
			}
			if (c == "")
			{
				c = "1";
			}
			else
			{
				if (c[0] == '*' || c[0] == '/' || c[0] == '^')
				{
					return false;
				}
				else
				{
					if (c[0] == '+' || c[0] == '-')
					{
						c = c.Remove(0, 1);
					}
					else
					{
						if (Char.IsDigit(c[0]))
						{
							return true;
						}
					}
				}

			}
			if (!Char.IsDigit(c.Last()))
			{
				c = c.Remove(c.Length - 1, 1);
			}
			return true;
		}

		public bool GetYes(List<Cell> listToken, ref string before1,ref string before2, ref string before3)
		{			
			int chast = 0;
			Cell token;
			for (int i = 0; i < listToken.Count; i++)
			{
				token = listToken[i];
				if (chast == 0)
				{					
					if (token.Type == "letter" && listToken[++i].Token == "^" && listToken[++i].Token == "2")
					{
						chast++;
						continue;
					}
					else
					{
						before1 += token.Type;
					}
				}				
				if (chast == 1)
				{
					if (token.Type == "letter" && listToken[++i].Type == "operation")
					{
						chast++;
						continue;
					}
					else
					{
						before2 += token.Token;
					}
				}
				if (chast == 2)
				{
					if (token.Type == "digit")
					{
						chast++;
						before3 += token.Token;
						continue;//запомнить token
					}
					else
					{
						before3 += token.Token;
					}
				}
				if (chast == 3)
				{
					if (token.Token == "=" && listToken[++i].Token == "0")
					{
						chast++;
						continue;
					}
					else
					{
						before3 += token.Token;
					}
				}
			}
			if (chast == 4)			
				return true;
			
			return false;
		}

		public struct Cell
		{
			internal Cell(string token, string type, int nomer)
			{
				Token = token;
				Type = type;
				Nomer = nomer;
			}

			internal string Token { get; set; }
			internal string Type { get; set; }
			internal int Nomer { get; set; }
		}

		static int nomerChislo = 0;
		static int nomerUnknown = 0;
		static int nomerOperation = 0;

		static List<Cell> BreakToken(string data)
		{
			int countParentthese = 0;
			List<Cell> listToken = new List<Cell>();
			string allToken = "";
			

			foreach (var ch in data)
			{
				
				if (ch == '=')
				{
					if (char.IsLetter(allToken[0]))
					{
						listToken.Add(new Cell(allToken, "letter", ++nomerUnknown));
					}
					else
					{
						listToken.Add(new Cell(allToken, "digit", ++nomerChislo));
					}
					listToken.Add(new Cell("=", "operation", ++nomerOperation));
					string[] qwert = data.Split(new char[] { '=' });

					listToken.Add(new Cell(qwert[1], "digit", ++nomerChislo));
					break;
				}
				 data.Remove(0, 1);
				if (Char.IsDigit(ch))
				{
					allToken += ch;
					continue;
				}
				if (Char.IsLetter(ch))
				{
					allToken += ch;
					continue;
				}
				else
				{
					if (allToken.Length != 0)
					{
						if (Char.IsLetter(allToken[0]))
						{
							listToken.Add(new Cell(allToken, "letter", ++nomerUnknown));
						}
						else
						{
							listToken.Add(new Cell(allToken, "digit", ++nomerChislo));
						}
					}
					if (ch == '(')
					{
						countParentthese++;
						listToken.Add(new Cell(ch.ToString() + countParentthese.ToString(), "operation", ++nomerOperation));
					}
					else
					{
						if (ch == ')')
						{
							listToken.Add(new Cell(ch.ToString() + countParentthese.ToString(), "operation", ++nomerOperation));
							countParentthese--;
							allToken = "";
						}
						else
						{
							listToken.Add(new Cell(ch.ToString(), "operation", ++nomerOperation));
							allToken = "";
						}
					}
				}
				
			}
			return listToken;
		}
	}
}
