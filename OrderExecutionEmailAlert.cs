//
// Copyright (C) 2022, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//
#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds strategies in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Strategies
{
	public class OrderExecutionEmailAlert : Strategy
	{
		private Account MyAccount;
 
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description	= "";
				Name		= "OrderExecutionEmailAlert";
					
		        // Find our Sim101 account
		        lock (Account.All)
		              MyAccount = Account.All.FirstOrDefault(a => a.Name == "Sim101");
		 
		        if (MyAccount != null)
		        {
			        MyAccount.ExecutionUpdate += OnExecutionUpdate;
		        }
			}
			else if (State == State.Terminated)
    		{
		        // Unsubscribe to events
				MyAccount.ExecutionUpdate -= OnExecutionUpdate;
			}
		}

		protected override void OnBarUpdate()
		{
			// noop
		}
		
		private void OnExecutionUpdate(object sender, ExecutionEventArgs e)
		{
			if (State == State.Historical)
			{
				return;
			}
			
		  	foreach (Execution execution in MyAccount.Executions)
		  	{
		      	Print(String.Format("Execution triggered for Order {0}", execution.Order));
				string message = String.Format("{0}", execution.Order);
		  		Share("Roboswap", message, new object[]{ "clockwork.roboswap@gmail.com", message });
			}
		}
		
		#region Properties
		#endregion
	}
}
