using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Sample.NetCore;
using SkiaSharp;

var builder = WebApplication.CreateBuilder(args);

// �ڴ�洢�� ����appsettings.json����
builder.Services.AddCaptcha(builder.Configuration, options =>
{
    // �Զ�������
    //options.ImageOption.FontSize = 28;
    //options.ImageOption.FontFamily = ResourceFontFamilysFinder.Find("KG HAPPY");
});
// �������������룬���ע�ͼ��ɡ�
//builder.Services.Add(ServiceDescriptor.Scoped<ICaptcha, RandomCaptcha>());

// Core��Ŀʹ�õ���IDistributedCache��
// ���ʹ��redis���棬��Ҫ��װ�� Microsoft.Extensions.Caching.StackExchangeRedis
// �ο���https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
//    options.InstanceName = "captcha:";
//});

// Core��Ŀʹ�õ���IDistributedCache��
// ���ʹ��SQLServer���棬��װ Microsoft.Extensions.Caching.SqlServer
//builder.Services.AddDistributedSqlServerCache(options =>
//{
//    options.ConnectionString = builder.Configuration.GetConnectionString(
//        "DistCache_ConnectionString");
//    options.SchemaName = "dbo";
//    options.TableName = "TestCache";
//});

// -----------------------------------------------------------------------------
// ȫ�����ò��������ڴ�������
builder.Services.AddCaptcha(builder.Configuration, option =>
{
    option.CaptchaType = CaptchaType.WORD; // ��֤������
    option.CodeLength = 6; // ��֤�볤��, Ҫ����CaptchaType���ú�.  ������Ϊ�������ʽʱ�����ȴ�������ĸ���
    option.ExpirySeconds = 30; // ��֤�����ʱ��
    option.IgnoreCase = true; // �Ƚ�ʱ�Ƿ���Դ�Сд
    option.StoreageKeyPrefix = ""; // �洢��ǰ׺

    option.ImageOption.Animation = true; // �Ƿ����ö���
    option.ImageOption.FrameDelay = 30; // ÿ֡�ӳ�,Animation=trueʱ��Ч, Ĭ��30

    option.ImageOption.Width = 150; // ��֤����
    option.ImageOption.Height = 50; // ��֤��߶�
    option.ImageOption.BackgroundColor = SKColors.White; // ��֤�뱳��ɫ

    option.ImageOption.BubbleCount = 2; // ��������
    option.ImageOption.BubbleMinRadius = 5; // ������С�뾶
    option.ImageOption.BubbleMaxRadius = 15; // �������뾶
    option.ImageOption.BubbleThickness = 1; // ���ݱ��غ��

    option.ImageOption.InterferenceLineCount = 2; // ����������

    option.ImageOption.FontSize = 36; // �����С
    option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // ���壬����ʹ��kaiti�������ַ��ɸ���ϲ�����ã����ܲ���ת�ַ�����ֻ��Ʋ������������

    option.ImageOption.TextBold = true;// ����
});

// ע�⣺ appsettings.json���úʹ�������ͬʱ����ʱ���������ûḲ��appsettings.json���á�

builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
