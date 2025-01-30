using Steamworks;
using Steamworks.Data;
using System;
using System.Runtime.InteropServices;

public class PacketSend
{
    public static Result Server_Send_test(NetworkPlayer pl)
    {
        using (packet p = new packet(0))
        {
            p.WriteUNICODE("怕錯借因乙浪菜躲青香浪但出雲，胡未而讀拉每棵乙干定送肖友田且給門，尺女少交，汁言米司北有青由上園孝服哭胡直嗎，這昔晚菜黑寺山朱現祖而，壯己麼得條習尺目東您畫耳見風個吹人什。從們寺、道尺里刀支。\r\n\r\n丟衣習片祖首光音禾豆或各斤也沒成，杯弟時在，頭是定發定前根也貫服夕只園帽、瓜造到土衣怕刃植才左黃怎未弓。刃着法，青法就和雲那去王能兔它。冬弓姊尼在珠來明邊五把半抱尼乾爸支少很害中品。\r\n\r\n丟告爪几娘消夕八手春爪但采童奶何！樹雲丁大至枝步？林聽肖次兌晚上請泉雲；遠科不貫千打個冒幾跑房哪了出車邊！年呀急菜即親寸走交。呀叫放。\r\n\r\n月放刀珠星經行圓故米抄自候、什刀四幫又尤原完品，葉花了文八巴屋爬走才目童貓兔聽別乙，它几食夏甲珠現京白別要自，澡玉清包是登戊四耍十物虎黃事的，九內時果行息急筆枝。\r\n\r\n斗園停珠首蛋品和借遠立申氣冬雪父：習喜英哥還；買兆戊歡，話有手巾食枝、常象爸好氣水，而東母收住立邊園笑夏車几誰爪裝月。\r\n\r\n喜高燈母，眼或兔衣汗從裏示邊斥村；路禾間洋誰口像拍公香旁您內，不息下她身地立造以巾問央象蝴比乍還米：菜現坡這急固品寸元化主首得久蝸跟，好聲幫。");
            return pl.SendPacket(p);

        };
    }
    public static Result Client_Send_test(Connection connection)
    {
        using (packet p = new packet(0))
        {
            p.WriteUNICODE("怕錯借因乙浪菜躲青香浪但出雲，胡未而讀拉每棵乙干定送肖友田且給門，尺女少交，汁言米司北有青由上園孝服哭胡直嗎，這昔晚菜黑寺山朱現祖而，壯己麼得條習尺目東您畫耳見風個吹人什。從們寺、道尺里刀支。\r\n\r\n丟衣習片祖首光音禾豆或各斤也沒成，杯弟時在，頭是定發定前根也貫服夕只園帽、瓜造到土衣怕刃植才左黃怎未弓。刃着法，青法就和雲那去王能兔它。冬弓姊尼在珠來明邊五把半抱尼乾爸支少很害中品。\r\n\r\n丟告爪几娘消夕八手春爪但采童奶何！樹雲丁大至枝步？林聽肖次兌晚上請泉雲；遠科不貫千打個冒幾跑房哪了出車邊！年呀急菜即親寸走交。呀叫放。\r\n\r\n月放刀珠星經行圓故米抄自候、什刀四幫又尤原完品，葉花了文八巴屋爬走才目童貓兔聽別乙，它几食夏甲珠現京白別要自，澡玉清包是登戊四耍十物虎黃事的，九內時果行息急筆枝。\r\n\r\n斗園停珠首蛋品和借遠立申氣冬雪父：習喜英哥還；買兆戊歡，話有手巾食枝、常象爸好氣水，而東母收住立邊園笑夏車几誰爪裝月。\r\n\r\n喜高燈母，眼或兔衣汗從裏示邊斥村；路禾間洋誰口像拍公香旁您內，不息下她身地立造以巾問央象蝴比乍還米：菜現坡這急固品寸元化主首得久蝸跟，好聲幫。");
            
            return PacketSendingUtils.SendPacketToConnection(connection,p);
            

        };
    }
}

public class PacketSendingUtils
{ 
    public static Result SendPacketToConnection(Connection c,packet p)
    {
        byte[] data = p.GetPacketData();
        IntPtr datapointer = Marshal.AllocHGlobal(data.Length);
        Marshal.Copy(data, 0, datapointer, data.Length);
        Result r = c.SendMessage(datapointer, data.Length, SendType.Reliable);
        Marshal.FreeHGlobal(datapointer);
        return r;
    }
}
