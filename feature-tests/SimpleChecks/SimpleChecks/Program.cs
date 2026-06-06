using SimpleChecks;
using SimpleLib;

Console.WriteLine("Press enter to execute actions...");
Console.ReadLine();

MainSvc svc = new MainSvc();
svc.ExecAction1();
MainSvc.ExecStaticAction();

MyUtils.ExecLibAction1();
int result = MyUtils.ExecLibAction2();

Console.WriteLine("Press enter to exit...");
Console.ReadLine();