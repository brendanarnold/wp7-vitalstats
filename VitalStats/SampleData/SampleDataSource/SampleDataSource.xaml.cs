﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SampleDataSource
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class SampleDataSource { }
#else

	public class SampleDataSource : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public SampleDataSource()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/VitalStats;component/SampleData/SampleDataSource/SampleDataSource.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private Profiles _Profiles = new Profiles();

		public Profiles Profiles
		{
			get
			{
				return this._Profiles;
			}
		}

		private SelectedProfile _SelectedProfile = new SelectedProfile();

		public SelectedProfile SelectedProfile
		{
			get
			{
				return this._SelectedProfile;
			}

			set
			{
				if (this._SelectedProfile != value)
				{
					this._SelectedProfile = value;
					this.OnPropertyChanged("SelectedProfile");
				}
			}
		}

		private MeasurementTypes _MeasurementTypes = new MeasurementTypes();

		public MeasurementTypes MeasurementTypes
		{
			get
			{
				return this._MeasurementTypes;
			}
		}
	}

	public class ProfilesItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private bool _IsProtected = false;

		public bool IsProtected
		{
			get
			{
				return this._IsProtected;
			}

			set
			{
				if (this._IsProtected != value)
				{
					this._IsProtected = value;
					this.OnPropertyChanged("IsProtected");
				}
			}
		}
	}

	public class Profiles : System.Collections.ObjectModel.ObservableCollection<ProfilesItem>
	{ 
	}

	public class SelectedProfile : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private Stats _Stats = new Stats();

		public Stats Stats
		{
			get
			{
				return this._Stats;
			}
		}

		private bool _IsProtected = false;

		public bool IsProtected
		{
			get
			{
				return this._IsProtected;
			}

			set
			{
				if (this._IsProtected != value)
				{
					this._IsProtected = value;
					this.OnPropertyChanged("IsProtected");
				}
			}
		}
	}

	public class StatsItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private string _Value = string.Empty;

		public string Value
		{
			get
			{
				return this._Value;
			}

			set
			{
				if (this._Value != value)
				{
					this._Value = value;
					this.OnPropertyChanged("Value");
				}
			}
		}

		private MeasurementType _MeasurementType = new MeasurementType();

		public MeasurementType MeasurementType
		{
			get
			{
				return this._MeasurementType;
			}

			set
			{
				if (this._MeasurementType != value)
				{
					this._MeasurementType = value;
					this.OnPropertyChanged("MeasurementType");
				}
			}
		}
	}

	public class Stats : System.Collections.ObjectModel.ObservableCollection<StatsItem>
	{ 
	}

	public class MeasurementTypesItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private bool _IsConvertible = false;

		public bool IsConvertible
		{
			get
			{
				return this._IsConvertible;
			}

			set
			{
				if (this._IsConvertible != value)
				{
					this._IsConvertible = value;
					this.OnPropertyChanged("IsConvertible");
				}
			}
		}
	}

	public class MeasurementTypes : System.Collections.ObjectModel.ObservableCollection<MeasurementTypesItem>
	{ 
	}

	public class MeasurementType : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private bool _IsConvertible = false;

		public bool IsConvertible
		{
			get
			{
				return this._IsConvertible;
			}

			set
			{
				if (this._IsConvertible != value)
				{
					this._IsConvertible = value;
					this.OnPropertyChanged("IsConvertible");
				}
			}
		}

		private Units _Units = new Units();

		public Units Units
		{
			get
			{
				return this._Units;
			}
		}
	}

	public class UnitsItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _Id = 0;

		public double Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.OnPropertyChanged("Id");
				}
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private string _ConversionFactor = string.Empty;

		public string ConversionFactor
		{
			get
			{
				return this._ConversionFactor;
			}

			set
			{
				if (this._ConversionFactor != value)
				{
					this._ConversionFactor = value;
					this.OnPropertyChanged("ConversionFactor");
				}
			}
		}

		private string _ConversionIntercept = string.Empty;

		public string ConversionIntercept
		{
			get
			{
				return this._ConversionIntercept;
			}

			set
			{
				if (this._ConversionIntercept != value)
				{
					this._ConversionIntercept = value;
					this.OnPropertyChanged("ConversionIntercept");
				}
			}
		}

		private string _Format = string.Empty;

		public string Format
		{
			get
			{
				return this._Format;
			}

			set
			{
				if (this._Format != value)
				{
					this._Format = value;
					this.OnPropertyChanged("Format");
				}
			}
		}
	}

	public class Units : System.Collections.ObjectModel.ObservableCollection<UnitsItem>
	{ 
	}
#endif
}
