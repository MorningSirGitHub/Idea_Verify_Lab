using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using System.IO;
using System.Collections;
public class Lang
{

    static Lang _instance;
    public static Lang Instance { get { if (_instance == null) _instance = new Lang(); return _instance; } }

    public static string CONFIRM = "确定";
    public static string CANCLE = "取消";
    public static string BACK = "返回";
    public static string YES = "是";
    public static string LOAD = "载入";
    public static string DESTROY = "销毁";
    public static string HOURS = "小时";
    public static string CANCONTROL = "可控制";
    public static string DOTCONTROL = "脱离控制";
    public static string GETBACK = "取  回";
    public static string BUTTON_REWARD_CAN = "领取";
    public static string BUTTON_REWARD_ALREADY = "已领取";
    public static string BUTTON_REWARD_DISABLE = "等级不足";
    public static string LACK_MONEY = "瓶盖不足";
    public static string TIP_TIME_ERR = "本地时间和服务器时间校验失败，请确认本地时间是否正确，请不要异常修改本地时间。";
    public static string TIP_TIME_ERR1 = "目前手机时间早于系统上次记录时间，请等待时间正常后再试。";
    public static string CONFIRM_CANCLE = "确定取消";
    public static string DO_IT_NEXTTIME = "下次再给";


    #region 物品相关

    public static string ITEM_ATTACK = "攻击";
    public static string ITEM_DEFENSE = "防御";
    public static string ITEM_SAN = "精力值";
    public static string ITEM_HEALTH = "健康值";
    public static string ITEM_HUNGER = "饱食度";
    public static string ITEM_WEIGHT = "负重";
    public static string ITEM_LUCK = "幸运";
    public static string ITEM_MAIN_WEAPON_ATTACK = "主武器攻击";
    public static string ITEM_DEPUTY_WEAPON_ATTACL = "副武器攻击";
    public static string ITEM_CLOSE_DEFENSE = "衣服防御力";
    public static string ITEM_PANT_HEALTH = "裤子健康值";
    public static string ITEM_SHOSE_HUNGER = "鞋子饱食度";

    public static string ITEM_EAT = "吃  掉";
    public static string ITEM_USE = "使  用";
    public static string ITEM_DRINK = "喝  掉";
    public static string ITEM_PUTON = "装  备";

    public static string ITEM_TYPE_MAINWEAPON = "主武器";
    public static string ITEM_TYPE_DEPUTYWEAPON = "副武器";
    public static string ITEM_TYPE_HAT = "头盔";
    public static string ITEM_TYPE_PACK = "背包";
    public static string ITEM_TYPE_PANTS = "裤子";
    public static string ITEM_TYPE_SHOES = "鞋子";
    public static string ITEM_TYPE_SUIT = "上衣";

    public static string ITEM_TYPE_DRINK = "饮料";
    public static string ITEM_TYPE_DRUG = "药物";
    public static string ITEM_TYPE_FOOD = "食物";
    public static string ITEM_TYPE_MATRIAL = "材料";
    public static string ITEM_TYPE_TOOL = "工具";
    public static string ITEM_TYPE_CONSUMABLE = "消耗品";
    public static string ITEM_TYPE_FIGHTPROPS = "战斗道具";
    public static string ITEM_TYPE_ROWMATERIAL = "原料";
    public static string ITEM_TYPE_CAP = "货币";

    #endregion

    #region 弹出的提示文字

    public static string POPTIP_GET_MORE_WIEGHT = "可以通过天赋，装备更好的背包等方式获得新的负重格。";
    public static string POPTIP_PACK_FULL_TRYAGAGI = "当前背包已满，请清理背包后再次尝试";
    public static string POPTIP_LACK_SAN = "精神值不足(还需 {0} 点)";
    public static string POPTIP_LACK_ITEM2 = "缺少物品：";
    public static string POPTIP_ITEM_HARM_HEALTH = "使用后会减少{0}生命值";
    public static string POPTIP_ITEM_HARM_HUNGER = "使用后会减少{0}饱食度";
    public static string POPTIP_ITEM_HARM_SAN = "使用后会减少{0}精神值";
    public static string POPTIP_UNOPEN = "功能还未开放！！";
    public static string POPTIP_PLAYER_STATE = "你目前处于虚弱状态，无法出门。\n是否使用<quad name=coin1 size=25 width=1 /> X {0}，立即治疗虚弱状态？\n你也可以等待时间自动恢复。";
    public static string POPTIP_CLICKOUT = "想到户外，请点击大门！";
    public static string POPTIP_TALENTLEVEL = "该天赋已经满级";
    public static string POPTIP_PLAYERLEVEL = "需要人物等级LV{0}";
    public static string POPTIP_TALENT_NEEDLEVLE = "需要{0}达到{1}";
    public static string POPTIP_TALENT_NEEDLEVEL_TWO = "需要{0}达到{1},需要{2}达到{3}";
    public static string POPTIP_TALENT_LACK = "缺少天赋点数：{0}点。";
    public static string POPTIP_DEALPANLE_COMPLETEDEAL = "交易完成了！！！";
    public static string POPTIP_DEALPANLE_MAXIMUM = "达到最大购买数量了";
    public static string POPTIP_DEALPANEL_SUPPLYFULL = "无法交易更多的物品！！";
    public static string POPTIP_GUNMODIFIEPANEL_NOGUN = "没有适合强化的枪支！！";
    public static string FIRSTSCENE_TIP = "稍等一会，马上好......";
    public static string ADVENTURE_DOTOPENBACK = "你现在没空打开背包";
    public static string CITY_SAMETIPS = "你已经在这个城市了";
    public static string POPTIP_CITY_TIMECOUNT = "迁移时间还在冷却中。。";
    public static string POPTIP_ACTIVITY_OVER = "活动已结束，请等待下一次活动开启！！";
    public static string DOTFUNCTION = "这个功能不能使用";
    public static string POPTIP_GET_ITEM_WAY = "工具台、制药台可制作战斗物品";
    public static string POPTIP_LACK_HEALTH = "生命值不足";
    public static string POPTIP_MUSICLES_LEVEL = "已达到当前最高等级";
    public static string POPTIP_NOT_HUNTER_RANK = "暂无可领取的奖励，你需要完整参与一轮狩猎排名！";
    public static string POPTIP_DEALPANEL_FULL = "交易失败，交易后背包溢出";
    public static string POPTIP_HUNGER_UP = "饱食度上升了";
    public static string POPTIP_HUNGER_DOWN = "饱食度下降了";
    public static string POPTIP_HEALTH_UP = "健康值上升了";
    public static string POPTIP_HEALTH_DOWN = "健康值下降了";
    public static string POPTIP_SPIRIYT_UP = "精神值上升了";
    public static string POPTIP_SPIRIYT_DOWN = "精神值下降了";
    public static string POPTIP_ADD_MAX = "一次最多选择3个";
    public static string POPTIP_NEED_SAN = "当前精力不足，还需{0}点。\n是否使用<quad name=coin1 size=25 width=1 /> X {1}，购买60点精力值？\n本日可购买次数（{2}/3）";
    public static string PopTip_NPC_Nobody = "此处主人已不在家，下次再来吧";
    #endregion

    #region 在面板上的提示文字

    public static string TIP_HEALTH_FULL = "健康值已满，是否继续使用该道具？";
    public static string TIP_HUNGER_FULL = "饱食度已满，是否继续使用该道具？";
    public static string TIP_SAN_FULL = "精神值已满，是否继续使用该道具？";
    public static string TIP_HUNTER_FRESH_MISSION = "刷新次数已用完。\n是否花费<quad name=coin1 size=25 width=1 /> X {0}刷新任务？";
    public static string TIP_GAME_QUIT = "是否退出游戏？";
    #endregion

    #region 女儿

    public static string DAUGHTER_WELCOME_HOME = "欢迎回来";
    public static string DAUGHTER_RECOVERTIP = "可以回复饱食度<color=#E4D206FF>{0}</color>点,心情<color=#E4D206FF>{1}</color>点";
    public static string DAUGHTER_XIXI = "嘻嘻嘻嘻";
    public static string DAUGHTER_QUEST_ITEM_CONFIRM = "要把身上的{0}给凯希吗？给了凯希可不还哦！\n凯希可是一个小气鬼！嘻嘻！";
    public static string DAUGHTER_QUEST_ITEM_CONFIRM2 = "确定要放弃给予凯希{0}物品吗？凯希虽然会难过，\n但只会埋在心底，不会说出来。";
    public static string KATHYCRIT = "凯希的帮助";
    public static string KATHYCRIT_CONTENT = "凯希的心情好坏决定她能不能帮你做事，比如她今天心情就不错特地去帮你照料了一下暖棚，\n因此你收获了更多的东西：{0} X {1}";
    public static string KATHYCRIT_CONTENT1 = "凯希的心情好坏决定她能不能帮你做事，比如她今天心情就不错特地去帮你照料了一下暖棚，\n因此你收获了更多的东西。";
    public static string KATHYCRIT_MOODNDHUN_ADD = "凯希的心情和饱食度增加了。";
    #endregion

    #region 背包

    public static string PACK_PUTINBACK = "放入仓库";

    #endregion

    #region 合成

    public static string COMPOSER_UPDATE = "升  级";
    public static string COMPOSER_BUILD = "制  造";
    public static string COMPOSER_REQUIRE_TIP = "需要消耗：精神 <color=#FFC125>{0}</color> 点，饱食度 <color=#FFC125>{1}</color> 点";
    public static string COMPOSER_ETC = "等";
    public static string COMPOSER_LOCK_TIP = "需要解锁<color=#FFC125>{0}</color>级";
    public static string COMPOSER_LOCK_TIP_KITCHIN = "需要解锁<color=#FFC125>{0}</color>级厨房";
    public static string COMPOSER_LOCK_TIP_TOOL = "需要解锁<color=#FFC125>{0}</color>级工具台";
    public static string COMPOSER_LOCK_TIP_DRUG = "需要解锁<color=#FFC125>{0}</color>级制药台";
    public static string COMPOSER_LOCK_TIP_PLANT = "需要解锁<color=#FFC125>{0}</color>级暖棚";
    public static string COMPOSER_LACK_HUNGER = "饱食度不足，是否消耗健康值 <color=#FFC125>{0}</color> 点代替";
    public static string COMPOSER_LACK_HEALTH_TIP = "健康值不足(还需 {0} 点健康值)";
    public static string COMPOSER_LACK_SAN_TIP = "精神值不足(还需 {0} 点精神值)";
    public static string COMPOSER_COMPLETE = "制作完成";
    public static string COMPOSER_BUILD_AGAIN = "再次制造";

    #endregion

    #region  人物

    public static string PLAYER_LEVEL_UP = "人物升级到了<color=#FFC125>{0}</color>级";
    public static string PLAYER_MOOD_UP = "心情值上升{0}点,现在心情为：";
    public static string PLAYER_MOOD_DOWN = "心情值下降{0}点,现在心情为：";
    public static string PLAYER_CONSUME_HUNGER = "消耗{0}点饱食度";
    public static string PLAYER_CONSUME_HEALTH = "消耗{0}点健康值";
    public static string PLAYER_CONSUME_SANITY = "消耗{0}点精力值";
    public static string PLAYER_LACK_SANITY = "精力值不足";
    public static string PLAYER_LOW_ATTACK = "等攻击力达到{0}时再来试试吧！";
    public static string PLAYER_LOW_HEALTH = "等健康值达到{0}时再来试试吧！";
    public static string PLAYER_LOW_SANITY = "等精神值达到{0}时再来试试吧！";
    public static string PLAYER_LOW_HUNGER = "等饱食度达到{0}时再来试试吧！";
    public static string PLAYER_LOW_DEFENCE = "等防御力达到{0}时再来试试吧！";
    public static string PLAYER_LOW_ITEM = "缺少{0}{1}个";
    public static string PLAYER_MOOD_REQUIRED = "等心情{0}时再来试试吧！";
    public static string PLAYER_MOOD_HAPPY = "愉快";
    public static string PLAYRE_MOOD_NORMAL = "正常";
    public static string PLAYER_MOOD_SAD = "沮丧";
    public static string PLAYER_MOOD_DESPAIR = "绝望";

    #endregion

    #region 探索

    public static string TOMB_EMPTY = "墓碑中空无一物";
    public static string LACK_OF_SOMETHING = "你没有足够的";
    public static string SOMETHING_BROKEN = "你消耗了{0}个";
    public static string WRONG_TIME = "时间不大对，试试{0}到{1}之间再来看看吧！";

    #endregion

    #region 战斗

    public static string PLAYER_FIGHT_OUTOFAMMO = "主枪子弹耗尽，自动切换成副枪";
    public static string PLAYER_FIGHT_WINEXPLAIN = "打扫战场获得物资：";
    public static string PLAYER_FIGHT_LOSEEXPLAIN = "昏迷后丢失物品：";
    public static string PLAYER_FIGHT_TARGETTASKLOSE = "目标任务失败：";
    public static string PLAYER_FIGHT_SETTINGBUTTONISOVERLAP = "按键重叠，请重新调整";
    public static string PLAYER_FIGHT_SETTINGSUCCESSFUL = "保存成功";

    #endregion

    #region 床铺
    public static string BUNK_BUILD = "建  造";
    public static string BUNK_REST = "休  息";
    public static string BUNK_SLEEP = "睡  觉";
    public static string BUNK_STOP = "停  止";
    public static string BUNK_SPVALE_TIP = "精力值缺少：{0}点,先补足精神吧！！";
    public static string BUNK_ITEM_TIP = "缺少：{0}X{1}个";
    public static string BUNK_TEXT_LOCKTIP = "点击建造按钮解锁{0}功能";
    public static string BUNK_TEXT_CONSUMETIP = "需要消耗：精力<color=#FFC125>{0}</color>点，饱食度<color=#FFC125>{1}</color>点";
    public static string BUNK_TEXT_LOCKBUNKNAME = "<color=#FFC125>{0}</color>尚未解锁";
    #endregion

    #region HomeGameControl
    public static string HOME_BUILD_DOGHOUSE = "狗  窝";
    public static string HOME_BUILD_RADIO = "电  台";
    public static string HOEM_BUILD_SOFA = "沙  发";
    public static string HOEM_BUILD_MACHINE = "制 药 台";
    public static string HOME_BUILD_KITCHEN = "厨  房";
    public static string HOME_BUILD_WORK = "工 具 台";
    public static string HOME_BUILD_VEGETABLES = "暖  棚";
    public static string HOEM_BUILD_BED = "床  铺";
    public static string PANELNAME_GUNMODIFIED = "枪械改装";
    public static string HOME_STORAGE = "仓  库";
    public static string HOME_DAUGHTER = "女  儿";
    public static string TALENT = "天  赋";
    public static string BACKPACK = "背  包";
    public static string HUNTER = "丧尸猎人";
    public static string DEAL_TITLE = "交  易";
    public static string CHEST_TITLE = "箱  子";
    #endregion

    #region 枪械改装



    #endregion

    #region 任务相关

    public static string TASK_TEXT_TIP = "<color=#FFC125>(已完成)</color>";
    public static string TASK_TEXT_TASKDESCORIBE = "{0}<color=#FF5F5FFF>(未完成)</color>";
    public static string TASK_TEXT_EXPAWARD = "<color=#F7FFB0FF>经验奖励：</color>{0}";
    public static string TASK_TEXT_DIALOG = "我：{0}";
    #endregion

    #region 探索界面

    public static string ADVENTURE_POPPANEL_DESCRIBE = "该区域十分危险，请在<color=#FFC125>{0}</color>级后再前往探索.";
    public static string ADVENTURE_POPPANEL_TIP = "道路被<color=#FFC125>{0}</color>阻隔,需要先探索完<color=#FFC125>{1}</color>.";
    public static string ADVENTURE_TEXT_PARAGRPH = "是否返回基地";
    public static string ADVENTURE_TEXT_TITLE = "回  家";
    public static string ADVENTURE_REGIONTIPS_LOW = "低";
    public static string ADVENTURE_REGIONTIPS_MIDDLE = "中";
    public static string ADVENTURE_REGIONTIPS_HIGH = "高";

    #endregion

    #region CharacterStatePanel

    public static string CHARACTERSTATE_TETX_AGGRESSIVITY = "<color=#00000>攻击力 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_PHYLACTIC = "<color=#00000>防御力 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_LUCK = "<color=#00000>幸运 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_BEAR = "<color=#00000>负重 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXHEALTH = "<color=#00000>最大健康值 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXHUNGER = "<color=#00000>最大饱食度 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXSANITY = "<color=#00000>最大精力值 </color><color=#FFCB6EFF>{0}</color>";

    public static string CHARACTERSTATE_TETX_AGGRESSIVITYVALUE = "<color=#00000>攻击力 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_PHYLACTICVALUE = "<color=#00000>防御力 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_LUCKVALUE = "<color=#00000>幸运 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_BEARVALUE = "<color=#00000>负重 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXHEALTHVALUE = "<color=#00000>最大健康值 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXHUNGERVALUE = "<color=#00000>最大饱食度 </color><color=#FFCB6EFF>{0}</color>";
    public static string CHARACTERSTATE_TETX_MAXSANITYVALUE = "<color=#00000>最大精力值 </color><color=#FFCB6EFF>{0}</color>";

    public static string CHARACTERSTATE_TEXT_MAINWEAPONSPEED = "<color=#00000>主武器射速 </color><color=#FFCB6EFF> 无 </color>";
    public static string CHARACTERSTATE_TEXT_MAINWEAPONRANGE = "<color=#00000>主武器射程 </color><color=#FFCB6EFF> 无 </color>";
    public static string CHARACTERSTATE_TEXT_MAINWEAPONAMMO = "<color=#00000>主武器弹药 </color><color=#FFCB6EFF> 无 </color>";

    public static string CHARACTERSTATE_TEXT_MAINWEAPONSPEEDVALUE = "<color=#00000>主武器射速 </color><color=#FFCB6EFF>{0} 发/秒</color>";
    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONSPEEDVALUE = "<color=#00000>副武器射速 </color><color=#FFCB6EFF>{0} 发/秒</color>";
    public static string CHARACTERSTATE_TEXT_MAINWEAPONRANGEVALUE = "<color=#00000>主武器射程 </color><color=#FFCB6EFF>{0} 米</color>";
    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONRANGEVALUE = "<color=#00000>副武器射程 </color><color=#FFCB6EFF>{0} 米</color>";

    public static string CHARACTERSTATE_TEXT_MAINWEAPONAMMOVALUE = "<color=#00000>主武器弹药 </color><color=#FFCB6EFF> {0} 发</color>";
    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONAMMOVALUE = "<color=#00000>副武器弹药 </color><color=#FFCB6EFF> {0} 发</color>";

    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONSPEED = "<color=#00000>副武器射速 </color><color=#FFCB6EFF> 无 </color>";
    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONRANGE = "<color=#00000>副武器射程 </color><color=#FFCB6EFF> 无 </color>";
    public static string CHARACTERSTATE_TEXT_DEPUTYWEAPONAMMO = "<color=#00000>副武器弹药 </color><color=#FFCB6EFF> 无 </color>";

    public static string TALENT_TEXT_REMAINPOINT = "剩余天赋点  <color=#0>{0}</color>";

    public static string TALENT_TEXT_TALENTSTATE = "未解锁";

    public static string TALENT_TEXT_TALENTDESCRIBE = "<color=#00>解锁需求：角色等级Lv</color><color=#FFE200FF>{0}</color>\n\n<color=#FFE200FF>当前未解锁</color>";
    public static string TALENT_TEXT_NEXTDESCRIBE = "<color=#FFE200FF>{0}：</color>{1}";
    public static string TALENT_TEXT_NEXTNEED = "<color=#00>下级需求：角色等级Lv</color><color=#FFE200FF>{0}</color>";

    public static string TALENT_TEXT_TALENTCURRNT = "<color=#00>解锁需求：</color><color=#FFE200FF>{0}Lv{1}</color>\n\n<color=#A55151FF>当前未解锁</color>";
    public static string TALENT_TEXT_NEXTNEED_TWO = "<color=#00>下级需求：</color><color=#FFE200FF>{0}LV{1}</color>";
    public static string TALENT_TEXT_TALENTDESCRIBE_TWO = "<color=#00>解锁需求：</color><color=#FFE200FF>{0}LV{1}  {2}LV{3}</color>\n\n<color=#A55151FF>当前未解锁</color>";
    public static string TALENT_TEXT_NEXTNEED_THREE = "<color=#00>下级需求：</color><color=#FFE200FF>{0}LV{1}  {2}LV{3}</color>";

    public static string TALRNT_TEXT_CONSUMETALENT = "<color=#EAD7B4FF>消耗：</color><color=#FFE200FF>天赋点</color>X{0}";

    public static string TALENT_TEXT_UNLOCK = "解  锁";

    public static string PLAYER_TEXT_DISEASESTATE_NONE = "无";
    public static string PLAYER_TEXT_DISEASESTATE_INDISPOSITION = "小病";
    public static string PLAYER_TEXT_DISEASESTATE_DISEASE = "重病";

    public static string PLYAER_TEXT_INJUREDSTATE_NONE = "无";
    public static string PLYAER_TEXT_INJUREDSTATE_MINORINJURY = "轻伤";
    public static string PLYAER_TEXT_INJUREDSTATE_SERIOUSINJURY = "重伤";

    public static string PLAYER_TETX_MOODSTATE_HAPPY = "愉快";
    public static string PLAYER_TETX_MOODSTATE_NORMAL = "正常";
    public static string PLAYER_TETX_MOODSTATE_DEPERSSION = "沮丧";
    public static string PLAYER_TETX_MOODSTATE_DESPAPIR = "绝望";



    #endregion

    #region CreatCharacterPanel
    public static string CREATCHATACTER_HAVEPLAYERDES = "楚门准备就绪，请控制者进行下一步操作。";
    public static string CREATCHATACTER_NOPLAYERDES = "是否激活新楚门，系统资源有限，请谨慎操作。";
    public static string CREATCHATACTER_ISDESTORYPLAYER = "控制者，您是否确认销毁这个楚门的副本？注意，本次操作不可逆！";
    public const string PLAYERNAEM = "楚门";

    #endregion

    #region DealPanel

    public static string DEALPANEL_TIPSDES = "你的东西太少了，对方不想和你交易。";
    public static string DEALPANEL_TIPSDES_TWO = "点击确定，我们可以完成交易了";

    #endregion

    #region NegotiatePanel

    public static string NEGOTIATEPANE_SHOWSELECT = "展示交涉策略";
    public static string NEGOTIATEPANE_SELECT = "请选择交涉策略";
    #endregion

    #region SettingsPanel
    public static string SETTINGPANEL_INPUTKEYS_NONE = "请输入礼品码";
    //public static string SETTINGPANEL_INPUTKEYS_WRONG = "礼品码错误";
    //public static string SETTINGPANEL_INPUTKEYS_HASBEENUSED = "礼品码已被使用";
    //public static string SETTINGPANEL_INPUTKEYS_HASBEENRECEIVED = "你已领取过此类礼品";
    public static string SETTINGPANLE_SIMPLECHINESE = "简体中文";
    public static string SETTINGPANLE_TRADITIONAL = "繁体中文";
    public static string SETTINGPANLE_JAPANESE = "日本语";
    #endregion

    #region TurenBoxPanel

    public static string TURNBOXPANLE_TIPSTEX = "现在离开，剩余物品就无法获得，你确定要离开么？";
    public static string TURNBOXPANLE_TIPSTEX_TWO = "你还有一次免费抽奖机会，是否要放弃？";
    public static string TURNBOXPANEL_MULTIPLE_THREE = "3倍奖励";
    public static string TURNBOXPANEL_MULTIPLE_FIVE = "5倍奖励";
    public static string TURNBOXPANEL_MULTIPLE_TEN = "10倍奖励";
    public static string TURNBOXPANEL_MULTIPLE_TWENTY = "20倍奖励";

    #endregion

    #region WeatherCoronaPanel

    public static string WEATHERCORONAPANEL_FOG = "大雾";
    public static string WEATHERCORONAPANEL_FINE = "晴天";
    public static string WEATHERCORONAPANEL_RAINSTORM = "暴雨";
    public static string WEATHERCORONAPANEL_BUTTONTIPS = "请按任意键，退出";
    #endregion

    #region 城市迁移

    public static string CITY_LACKMATERIALS_TIPS = "缺少：{0}X{1}个";
    public static string CITY_TRANSFER = "迁  移";
    public static string ACTIVITY_LEVELTIPS = "等级需要达到：{0},先提升等级再过去吧！！";
    public static string ACTIVITY_ENERGYTIPS = "缺少精力值：{0}点，养足精神再去吧！！";
    public static string ADVENTURE_AWARDPOP_DESCRIBE = "在刚刚的探索中，寻找到了前往这些新区域的道路。";
    #endregion

    #region 肌肉强化

    public static string MUSICLES_TITLENAME = "肌肉强化";
    public static string MUSCILES_CURRENTLV = "当前等级<color=#FFC125>Lv{0}</color>";
    public static string MUSCILES_NEXTLV = "下次升级<color=#FFC125>Lv{0}</color>";
    public static string MUSICLES_BUTTON_COMMON = "普通锻炼";
    public static string MUSICLES_BUTTON_SPECIAL = "快速锻炼";
    public static string MUSICLES_SUCCEEPROB = "成功概率<color=#FFC125>{0}%</color>";

    public static string MUSICLES_SHOULDER = "肩膀+{0}";
    public static string MUSICLES_ARM = "上臂+{0}";
    public static string MUSICLES_CHEST = "胸肌+{0}";
    public static string MUSICLES_REAR = "背部+{0}";
    public static string MUSICLES_ABDOMINAL = "腹肌+{0}";
    public static string MUSICLES_LEG = "腿部+{0}";

    public static string MUSICLES_SUGGESTTIPS = "30级标准强化等级";
    public static string MUSICLES_CUSTOMTIPS = "人物强化等级";

    public static string MUSICLES_SHOULDERNAME = "肩膀";
    public static string MUSICLES_ARMNAME = "上臂";
    public static string MUSICLES_CHESTNAME = "胸肌";
    public static string MUSICLES_REARNAME = "背部";
    public static string MUSICLES_ABDOINALNAME = "腹部";
    public static string MUSICLES_LEGNAME = "腿部";

    public static string MUSICLES_VALUEDESCRIBE_ONE = "攻击力增加";
    public static string MUSICLES_VALUEDESCRIBE_TWO = "防御力增加";
    public static string MUSICLES_VALUEDESCRIBE_THREE = "健康值最大值增加";
    public static string MUSICLES_VALUEDESCRIBE_FOUR = "饱食度最大值增加";
    public static string MUSICLES_VALUEDESCRIBE_FIVE = "精神值最大值增加";

    public static string MUSICLES_PERDESCRIBE_ONE = "攻击力百分比增加";
    public static string MUSICLES_PERDESCRIBE_TWO = "防御力百分比增加";
    public static string MUSICLES_PERDESCRIBE_THREE = "健康值百分比增加";
    public static string MUSICLES_PERDESCRIBE_FOUR = "饱食度百分比增加";
    public static string MUSICLES_PERDESCRIBE_FIVE = "精神值百分比增加";

    public static string MUSICLES_MATERIALREMAIN = "剩余";
    #endregion

    #region 丧失猎人

    public static string HUNTER_MISSION_STARTTIP = "是否开始执行{0}任务？";
    public static string HUNTER_TEAMMATER_NAME = "匿名玩家";
    public static string HUNTER_RANKREWARD_TIP = "你上一次在猎人小队里的排名是第{0}名，收下这些奖励吧：";
    public static string HUNTER_GET_REWARDTIP = "奖励领取成功，我们会直接送去你的住所。";
    public static string HUNTER_RANK_SPACE = "猎人小队重新编组中，请明天再来。";


    #endregion

    #region 探索地图界面

    public static string ADVENTUREMAP_PROGRE = "探明事件：{0}/{1}";

    #endregion

    #region TurnBoxPabel
    public static string TURNBOXPANEL_OPENTIPS_BOX = "箱子已经打开过了，请点击其它箱子。";
    public static string TURNBOXPANEL_OPENTIPS_COIN = "你的瓶盖不够用啦";
    #endregion

    #region Pay
    public static string PAY_ACTIVITY_TITLE = "连续登录以获得丰厚奖励";
    public static string Pay_Activity_Title_Grow = "楚门等级达到时可获得";
    public static string PAY_ACTIVITY_NAME = "活  动";
    public static string PAY_ACTIVITY_TIMETIP = "过{0} 后 登陆领取奖励";
    public static string PAY_ACTIVITY_FIRSTDAY = "第一天";
    public static string PAY_ACTIVITY_SECONDDAY = "第二天";
    public static string PAY_ACTIVITY_THREEDAY = "第三天";
    public static string PAY_ACTIVITY_FOUREDAY = "第四天";
    public static string PAY_ACTIVITY_FIVEDAY = "第五天";
    public static string PAY_ACTIVITY_SIXDAY = "第六天";
    public static string PAY_ACTIVITY_SEVENDAY = "第七天";
    public static string PAY_ACTIVITY_GETREWARD = "领取奖励";
    public static string PAY_ACTIVITY_FREE = "免  费";
    public static string PAY_ACTIVITY_NORESTRICTION = "不限量";
    public static string PAY_ACTIVITY_SUPPLYTITLE = "购买杂货";
    public static string PAY_ACTIVITY_SUPPLYBUNMBERTIP = "购买数量";
    public static string PAY_ACTIVITY_SUPPLY_CURRENTHAVE = "当前拥有<color=#FFC125>{0}</color>";
    public static string PAY_ACTIVITY_DISCOUNT = "{0}折";
    public static string PAY_ACTIVITY_SUMVALUE = "总价         <color=#FFC125>{0}</color>";
    public static string PAY_ACTIVITY_STACK = "库存:{0}";
    public static string PAY_ACTIVITY_BUYSUCCEED = "购买成功";
    public static string PAY_ACTIVITY_REFRESHTIME = "刷新时间：<color=#FFC125>{0}</color>";
    public static string PAY_RECHARGE_TIPS = "累计充值可获得:";
    public static string PAY_RECHARGE_BTNNAME = "前往充值";
    public static string PAY_RECHARGE_Name = "福  利";
    public static string PAY_RECHARGE_REWARDNAME = "领取奖励";
    public static string PAY_RECHARGE_REWARDTITLE = "获得物品";
    public static string PAY_RECHARGE_REWARDNTNNAME = "确  定";

    public static string PAY_SUPPLY_NAME = "补  给";
    public static string PAY_Supply_CAPNAME = "瓶盖";
    public static string PAY_SUPPLY_GOODSNAME = "物资";
    public static string PAY_SUPPLY_INDENTITY = "身份";
    public static string PAY_SUPPLY_BUYTIP = "购买获得";

    public static string Pay_Rechare_ExtraBottleNumber = "额外送{0}";
    public static string Pay_Recharge_Money = "{0}元";

    public static string Pay_Recharge_BuyTip = "购买获得";
    public static string PAY_LOW_CAPNAME = "瓶盖不足";
    public static string Pay_Resupply_PackType11 = "限时抢购";
    public static string Pay_Resupply_PackType12 = "特价物资";
    public static string Pay_Resupply_CapNumShortage = "瓶盖数量不足";
    public static string Pay_Resupply_NoPackTips = "资源稀缺，等待下次投放";
    public static string Pay_Resupply_CountTime = "倒计时:{0}";
    public static string Pay_Recharge_GetReward = "奖励领取成功！！";
    public static string Pay_MonthCard_ExtriyTips = "每日额外赠送{0}";
    public static string Pay_Monthcard_Tips = "立即获得{0}";
    public static string Pay_Monthcard_Dailygift = "今日可领取{0}";
    public static string Pay_Monthcard_BlockCardName = "末世中珍贵身份卡";
    public static string Pay_Monthcard_GreenCardName = "末世中居民身份卡";
    public static string Pay_Monthcard_BlockCardValidity = "永久有效";
    public static string Pay_Monthcard_GreenCardValidity = "有效期30天";
    public static string Pay_Monthcard_RemainValidity = "剩余<color=#B80000FF>{0}</color>天";


    public static string Pay_ActivityGrow_Name = "{0}级奖励";

    #endregion



    public bool IsDone { get; private set; }

    Dictionary<string, string> ContentList = new Dictionary<string, string>();

    /// <summary>
    /// 转换当前的语言
    /// </summary>
    public void Convert()
    {
        IsDone = false;
        //GameControl.Instance.StartCoroutine(ConvertProcess());
    }

    IEnumerator ConvertProcess()
    {
        var allItems = GetAllTips(this);

        // Debug.Log(Lang.Instance.str1);
        string savePath = "file:///" + Application.streamingAssetsPath + "/Data/Language/NewestTranslateText_Code.xls";
#if UNITY_EDITOR
        WWW www = new WWW(savePath);

#elif UNITY_ANDROID
        savePath = Application.streamingAssetsPath + "/Data/Language/NewestTranslateText.xlsx";

       WWW www = new WWW(savePath);
#endif
        yield return www;

        string[] lines = Encoding.UTF8.GetString(www.bytes).Split(new string[] { "\r\n" }, StringSplitOptions.None);
        //string[] lines = Encoding.GetEncoding("GB2312").GetString(www.bytes).Split(new string[] { "\r\n" }, StringSplitOptions.None);
        // var lines = File.ReadAllLines(savePath, Encoding.GetEncoding("GB2312"));
        //  Encoding.UTF8.GetString(www.bytes);

        if (lines.Length <= 0)
        {
            Debug.LogErrorFormat("翻译文件没找到：{0}", savePath);
            yield break;
        }

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var temp = line.Split('\t');
            if (temp.Length == 1)
            {
                Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
                continue;
            }
            else if (temp.Length == 2)
            {
                if (string.IsNullOrEmpty(temp[1]))
                {
                    Debug.LogErrorFormat("这一行没有翻译：{0}", temp[0]);
                    continue;
                }
            }
            if (ContentList.ContainsKey(temp[0]))
            {
                Debug.LogError("存在相同的KEY  " + temp[0]);
                continue;
            }
            ContentList.Add(temp[0], temp[1]);
            //Debug.Log(temp[1]);
        }

        string[] keyList = new string[allItems.Count];
        string[] valueList = new string[allItems.Count];
        allItems.Keys.CopyTo(keyList, 0);
        allItems.Values.CopyTo(valueList, 0);

        foreach (var key in keyList)
        {
            if (ContentList.ContainsKey(allItems[key]))
            {
                allItems[key] = ContentList[allItems[key]];
                if (allItems[key].Contains(" "))
                {
                    allItems[key] = allItems[key].Replace(" ", StringUtility.no_breaking_space);
                }
            }
            else
            {
                Debug.LogErrorFormat("存在没有找到翻译的字段：{0}", allItems[key]);
            }
        }

        LoadLangPreference(allItems, this);
        IsDone = true;
    }

    /// <summary>
    /// 获取当前类的public字段
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetAllTips<T>(T t) where T : class
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        FieldInfo[] fields = t.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            try
            {
                result.Add(field.Name, field.GetValue(this).ToString());

            }
            catch
            {
                Debug.LogError(field.GetValue(this).ToString());
            }
        }
        return result;
    }

    /// <summary>
    /// 设置当前类的public字段
    /// </summary>
    /// <param name="langPre"></param>
    public void LoadLangPreference<T>(Dictionary<string, string> langPre, T t) where T : class
    {
        FieldInfo[] fields = t.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            if (langPre.ContainsKey(field.Name))
            {
                try
                {
                    field.SetValue(this, langPre[field.Name]);

                }
                catch
                {
                    Debug.LogError(langPre[field.Name]);
                }
            }
        }
    }

}