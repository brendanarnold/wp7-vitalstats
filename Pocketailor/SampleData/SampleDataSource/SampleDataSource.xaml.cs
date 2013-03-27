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
				System.Uri resourceUri = new System.Uri("/Pocketailor;component/SampleData/SampleDataSource/SampleDataSource.xaml", System.UriKind.Relative);
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

		private StatTemplates _StatTemplates = new StatTemplates();

		public StatTemplates StatTemplates
		{
			get
			{
				return this._StatTemplates;
			}
		}

		private SuggestedStatTemplate _SuggestedStatTemplate = new SuggestedStatTemplate();

		public SuggestedStatTemplate SuggestedStatTemplate
		{
			get
			{
				return this._SuggestedStatTemplate;
			}

			set
			{
				if (this._SuggestedStatTemplate != value)
				{
					this._SuggestedStatTemplate = value;
					this.OnPropertyChanged("SuggestedStatTemplate");
				}
			}
		}

		private SelectedStat _SelectedStat = new SelectedStat();

		public SelectedStat SelectedStat
		{
			get
			{
				return this._SelectedStat;
			}

			set
			{
				if (this._SelectedStat != value)
				{
					this._SelectedStat = value;
					this.OnPropertyChanged("SelectedStat");
				}
			}
		}

		private QuickProfiles _QuickProfiles = new QuickProfiles();

		public QuickProfiles QuickProfiles
		{
			get
			{
				return this._QuickProfiles;
			}
		}

		private ConversionsByRegion _ConversionsByRegion = new ConversionsByRegion();

		public ConversionsByRegion ConversionsByRegion
		{
			get
			{
				return this._ConversionsByRegion;
			}
		}

		private Regions _Regions = new Regions();

		public Regions Regions
		{
			get
			{
				return this._Regions;
			}
		}

		private bool _IsLocked = false;

		public bool IsLocked
		{
			get
			{
				return this._IsLocked;
			}

			set
			{
				if (this._IsLocked != value)
				{
					this._IsLocked = value;
					this.OnPropertyChanged("IsLocked");
				}
			}
		}

		private System.Windows.Media.ImageSource _ConversionsByRegionPageBGImage = null;

		public System.Windows.Media.ImageSource ConversionsByRegionPageBGImage
		{
			get
			{
				return this._ConversionsByRegionPageBGImage;
			}

			set
			{
				if (this._ConversionsByRegionPageBGImage != value)
				{
					this._ConversionsByRegionPageBGImage = value;
					this.OnPropertyChanged("ConversionsByRegionPageBGImage");
				}
			}
		}

		private string _ConversionsByRegionPageTitle = string.Empty;

		public string ConversionsByRegionPageTitle
		{
			get
			{
				return this._ConversionsByRegionPageTitle;
			}

			set
			{
				if (this._ConversionsByRegionPageTitle != value)
				{
					this._ConversionsByRegionPageTitle = value;
					this.OnPropertyChanged("ConversionsByRegionPageTitle");
				}
			}
		}

		private string _SelectedConversionType = string.Empty;

		public string SelectedConversionType
		{
			get
			{
				return this._SelectedConversionType;
			}

			set
			{
				if (this._SelectedConversionType != value)
				{
					this._SelectedConversionType = value;
					this.OnPropertyChanged("SelectedConversionType");
				}
			}
		}
	}

	public class Profiles : System.Collections.ObjectModel.ObservableCollection<ProfilesItem>
	{ 
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

		private string _Gender = string.Empty;

		public string Gender
		{
			get
			{
				return this._Gender;
			}

			set
			{
				if (this._Gender != value)
				{
					this._Gender = value;
					this.OnPropertyChanged("Gender");
				}
			}
		}
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

		private Stats _Stats = new Stats();

		public Stats Stats
		{
			get
			{
				return this._Stats;
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

		private string _Gender = string.Empty;

		public string Gender
		{
			get
			{
				return this._Gender;
			}

			set
			{
				if (this._Gender != value)
				{
					this._Gender = value;
					this.OnPropertyChanged("Gender");
				}
			}
		}
	}

	public class Stats : System.Collections.ObjectModel.ObservableCollection<StatsItem>
	{ 
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

		private PreferredUnit _PreferredUnit = new PreferredUnit();

		public PreferredUnit PreferredUnit
		{
			get
			{
				return this._PreferredUnit;
			}

			set
			{
				if (this._PreferredUnit != value)
				{
					this._PreferredUnit = value;
					this.OnPropertyChanged("PreferredUnit");
				}
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

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
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

		private Units _Units = new Units();

		public Units Units
		{
			get
			{
				return this._Units;
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

	public class Units : System.Collections.ObjectModel.ObservableCollection<UnitsItem>
	{ 
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

	public class PreferredUnit : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
	}

	public class MeasurementTypes : System.Collections.ObjectModel.ObservableCollection<MeasurementTypesItem>
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

	public class StatTemplates : System.Collections.ObjectModel.ObservableCollection<StatTemplatesItem>
	{ 
	}

	public class StatTemplatesItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private MeasurementType3 _MeasurementType = new MeasurementType3();

		public MeasurementType3 MeasurementType
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

		private PreferredUnit2 _PreferredUnit = new PreferredUnit2();

		public PreferredUnit2 PreferredUnit
		{
			get
			{
				return this._PreferredUnit;
			}

			set
			{
				if (this._PreferredUnit != value)
				{
					this._PreferredUnit = value;
					this.OnPropertyChanged("PreferredUnit");
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
	}

	public class MeasurementType3 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private Units3 _Units = new Units3();

		public Units3 Units
		{
			get
			{
				return this._Units;
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
	}

	public class Units3 : System.Collections.ObjectModel.ObservableCollection<UnitsItem3>
	{ 
	}

	public class UnitsItem3 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
	}

	public class PreferredUnit2 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
	}

	public class SuggestedStatTemplate : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private MeasurementType1 _MeasurementType = new MeasurementType1();

		public MeasurementType1 MeasurementType
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
	}

	public class MeasurementType1 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private Units1 _Units = new Units1();

		public Units1 Units
		{
			get
			{
				return this._Units;
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

	public class Units1 : System.Collections.ObjectModel.ObservableCollection<UnitsItem1>
	{ 
	}

	public class UnitsItem1 : System.ComponentModel.INotifyPropertyChanged
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

		private double _ConversionFactor = 0;

		public double ConversionFactor
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

		private double _ConversionIntercept = 0;

		public double ConversionIntercept
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
	}

	public class SelectedStat : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private MeasurementType2 _MeasurementType = new MeasurementType2();

		public MeasurementType2 MeasurementType
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

		private PreferredUnit1 _PreferredUnit = new PreferredUnit1();

		public PreferredUnit1 PreferredUnit
		{
			get
			{
				return this._PreferredUnit;
			}

			set
			{
				if (this._PreferredUnit != value)
				{
					this._PreferredUnit = value;
					this.OnPropertyChanged("PreferredUnit");
				}
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

		private double _Value = 0;

		public double Value
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
	}

	public class MeasurementType2 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private Units2 _Units = new Units2();

		public Units2 Units
		{
			get
			{
				return this._Units;
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

	public class Units2 : System.Collections.ObjectModel.ObservableCollection<UnitsItem2>
	{ 
	}

	public class UnitsItem2 : System.ComponentModel.INotifyPropertyChanged
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

		private double _ConversionFactor = 0;

		public double ConversionFactor
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

		private double _ConversionIntercept = 0;

		public double ConversionIntercept
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

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
	}

	public class PreferredUnit1 : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
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
	}

	public class QuickProfiles : System.Collections.ObjectModel.ObservableCollection<QuickProfilesItem>
	{ 
	}

	public class QuickProfilesItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Gender = string.Empty;

		public string Gender
		{
			get
			{
				return this._Gender;
			}

			set
			{
				if (this._Gender != value)
				{
					this._Gender = value;
					this.OnPropertyChanged("Gender");
				}
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
	}

	public class ConversionsByRegion : System.Collections.ObjectModel.ObservableCollection<ConversionsByRegionItem>
	{ 
	}

	public class ConversionsByRegionItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private Conversions _Conversions = new Conversions();

		public Conversions Conversions
		{
			get
			{
				return this._Conversions;
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
	}

	public class Conversions : System.Collections.ObjectModel.ObservableCollection<ConversionsItem>
	{ 
	}

	public class ConversionsItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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

		private string _FormattedValue = string.Empty;

		public string FormattedValue
		{
			get
			{
				return this._FormattedValue;
			}

			set
			{
				if (this._FormattedValue != value)
				{
					this._FormattedValue = value;
					this.OnPropertyChanged("FormattedValue");
				}
			}
		}
	}

	public class Regions : System.Collections.ObjectModel.ObservableCollection<RegionsItem>
	{ 
	}

	public class RegionsItem : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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
	}
#endif
}
