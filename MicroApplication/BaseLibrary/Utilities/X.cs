using BaseLibrary.Utilities.DataCleaners;
using BaseLibrary.Utilities.DataGenerators;
using BaseLibrary.Utilities.Encode;
using BaseLibrary.Utilities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UrlEncoder = BaseLibrary.Utilities.Encode.UrlEncoder;

namespace BaseLibrary.Utilities
{
    public class CodeHelper
    {
        public static string CallingMethodInfo([CallerMemberName] string memberName = "",
[CallerFilePath] string fileName = "",
[CallerLineNumber] int lineNumber = 0)
        {
            return fileName + "-" + memberName;
        }
    }
    public static class X
    {
        public static IDatabaseLogger? Logger { get; set; }
        public static Encoder Encoder = new Encoder();
        public static Extension Extension = new Extension();
        public static InfoExtractor InfoExtractor = new InfoExtractor();
        public static DataGenerator DataGenerator = new DataGenerator();
        public static DataCleaner DataCleaner = new DataCleaner();
        public static Security Security = new Security();
        public static Validator Validator = new Validator();
        public static MathUtility Math = new MathUtility();
        public static FileUtility File = new FileUtility();
    }

    public class FileUtility
    {
        AssemblyEmbededFileReader assemblyEmbededFileReader;
        public AssemblyEmbededFileReader AssemblyEmbededFileReader { get { return assemblyEmbededFileReader ?? (assemblyEmbededFileReader = new AssemblyEmbededFileReader()); } }

        AssemblyTextFileReader assemblyTextFileReader;
        public AssemblyTextFileReader AssemblyTextFileReader { get { return assemblyTextFileReader ?? (assemblyTextFileReader = new AssemblyTextFileReader()); } }
    }


    public class Encoder
    {
        UrlEncoder urlEncoder;
        public UrlEncoder UrlEncoder { get { return urlEncoder ?? (urlEncoder = new UrlEncoder()); } }

        WebsiteEncoder websiteEncoder;
        public WebsiteEncoder WebsiteEncoder { get { return websiteEncoder ?? (websiteEncoder = new WebsiteEncoder()); } }

        FileNameEncoder fileNameEncoder;
        public FileNameEncoder FileNameEncoder { get { return fileNameEncoder ?? (fileNameEncoder = new FileNameEncoder()); } }

    }

    

    public class InfoExtractor
    {
        WebsiteLinkExtractor _websiteLink;
        public WebsiteLinkExtractor WebsiteLink { get { return _websiteLink ?? (_websiteLink = new WebsiteLinkExtractor()); } }

        EmailExtractor _email;
        public EmailExtractor Email { get { return _email ?? (_email = new EmailExtractor()); } }

        PhoneExtractor _phone;
        public PhoneExtractor Phone { get { return _phone ?? (_phone = new PhoneExtractor()); } }
    }
    public class Validator
    {
        CityNameValidator cityNameValidator;
        public CityNameValidator CityNameValidator
        {
            get
            {
                if (this.cityNameValidator == null)
                    this.cityNameValidator = new CityNameValidator();
                return this.cityNameValidator;
            }
        }

        DesignationValidator designationValidator;
        public DesignationValidator DesignationValidator
        {
            get
            {
                if (this.designationValidator == null)
                    this.designationValidator = new DesignationValidator();
                return this.designationValidator;
            }
        }

        ContactNumbereValidator contactNumbereValidator;
        public ContactNumbereValidator ContactNumbereValidator
        {
            get
            {
                if (this.contactNumbereValidator == null)
                    this.contactNumbereValidator = new ContactNumbereValidator();
                return this.contactNumbereValidator;
            }
        }

        NameValidator nameValidator;
        public NameValidator NameValidator
        {
            get
            {
                if (this.nameValidator == null)
                    this.nameValidator = new NameValidator();
                return this.nameValidator;
            }
        }

        EmailValidator emailValidator;
        public EmailValidator EmailValidator
        {
            get
            {
                if (this.emailValidator == null)
                    this.emailValidator = new EmailValidator();
                return this.emailValidator;
            }
        }

        StringValidator stringValidator;
        public StringValidator StringValidator
        {
            get
            {
                if (this.stringValidator == null)
                    this.stringValidator = new StringValidator();
                return this.stringValidator;
            }
        }

        IntegerValidator integerValidator;
        public IntegerValidator IntegerValidator
        {
            get
            {
                if (this.integerValidator == null)
                    this.integerValidator = new IntegerValidator();
                return this.integerValidator;
            }
        }

        DecimalValidator decimalValidator;
        public DecimalValidator DecimalValidator
        {
            get
            {
                if (this.decimalValidator == null)
                    this.decimalValidator = new DecimalValidator();
                return this.decimalValidator;
            }
        }

        GuidValidator guidValidator;
        public GuidValidator GuidValidator
        {
            get
            {
                if (this.guidValidator == null)
                    this.guidValidator = new GuidValidator();
                return this.guidValidator;
            }
        }

        LanguageValidator languageValidator;
        public LanguageValidator LanguageValidator
        {
            get
            {
                if (this.languageValidator == null)
                    this.languageValidator = new LanguageValidator();
                return this.languageValidator;
            }
        }

        TimeZoneValidator timeZoneValidator;
        public TimeZoneValidator TimeZoneValidator
        {
            get
            {
                if (this.timeZoneValidator == null)
                    this.timeZoneValidator = new TimeZoneValidator();
                return this.timeZoneValidator;
            }
        }

        CountryPhoneNumberValidator countryPhoneNumberValidator;
        public CountryPhoneNumberValidator CountryPhoneNumberValidator
        {
            get
            {
                if (this.countryPhoneNumberValidator == null)
                    this.countryPhoneNumberValidator = new CountryPhoneNumberValidator();
                return this.countryPhoneNumberValidator;
            }
        }
        DoubleValidator doubleValidator;
        public DoubleValidator DoubleValidator
        {
            get
            {
                if (this.doubleValidator == null)
                    this.doubleValidator = new DoubleValidator();
                return this.doubleValidator;
            }
        }

    }

    public class MathUtility
    {

        PercentageCalculator percentageCalculator;
        public PercentageCalculator Percentage { get { return percentageCalculator ?? (percentageCalculator = new PercentageCalculator()); } }

    }
}
