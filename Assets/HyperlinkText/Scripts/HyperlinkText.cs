using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    //[RequireComponent(typeof(UIVertexOptimize))]
    public class HyperlinkText : Text, IPointerClickHandler
    {
        // NOTE: 目前不支持嵌套
        // TODO: 待优化、超链接里面输入换行符会导致内部计算的字符与顶点不一致

        #region -----------------------------------------> 内部字段 <------------------------------------------

        protected string m_HyperOutputText;
        protected const string m_RegexEmoji = @"(?:\uD83D(?:\uDD73\uFE0F?|\uDC41(?:(?:\uFE0F(?:\u200D\uD83D\uDDE8\uFE0F?)?|\u200D\uD83D\uDDE8\uFE0F?))?|[\uDDE8\uDDEF]\uFE0F?|\uDC4B(?:\uD83C[\uDFFB-\uDFFF])?|\uDD90(?:(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F))?|[\uDD96\uDC4C\uDC48\uDC49\uDC46\uDD95\uDC47\uDC4D\uDC4E\uDC4A\uDC4F\uDE4C\uDC50\uDE4F\uDC85\uDCAA\uDC42\uDC43\uDC76\uDC66\uDC67](?:\uD83C[\uDFFB-\uDFFF])?|\uDC71(?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2640\u2642]\uFE0F?))?)|\u200D(?:[\u2640\u2642]\uFE0F?)))?|\uDC68(?:(?:\uD83C(?:\uDFFB(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFC-\uDFFF]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFC(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFD(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFE(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFF(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFE]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?)|\u200D(?:\uD83E[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD]|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D(?:\uDC69\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|\uDC68\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92])|\u2708\uFE0F?|\u2764(?:\uFE0F\u200D\uD83D(?:\uDC8B\u200D\uD83D\uDC68|\uDC68)|\u200D\uD83D(?:\uDC8B\u200D\uD83D\uDC68|\uDC68)))))?|\uDC69(?:(?:\uD83C(?:\uDFFB(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC69\uD83C[\uDFFC-\uDFFF]|\uDC68\uD83C[\uDFFC-\uDFFF])|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFC(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC69\uD83C[\uDFFB\uDFFD-\uDFFF]|\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF])|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFD(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC69\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFE(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC69\uD83C[\uDFFB-\uDFFD\uDFFF]|\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF])|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?|\uDFFF(?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC69\uD83C[\uDFFB-\uDFFE]|\uDC68\uD83C[\uDFFB-\uDFFE])|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?)|\u200D(?:\uD83E[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD]|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D(?:\uDC69\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92])|\u2708\uFE0F?|\u2764(?:\uFE0F\u200D\uD83D(?:\uDC8B\u200D\uD83D[\uDC68\uDC69]|[\uDC68\uDC69])|\u200D\uD83D(?:\uDC8B\u200D\uD83D[\uDC68\uDC69]|[\uDC68\uDC69])))))?|[\uDC74\uDC75](?:\uD83C[\uDFFB-\uDFFF])?|[\uDE4D\uDE4E\uDE45\uDE46\uDC81\uDE4B\uDE47\uDC6E](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|\uDD75(?:(?:\uFE0F(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDC82\uDC77](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|\uDC78(?:\uD83C[\uDFFB-\uDFFF])?|\uDC73(?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDC72\uDC70\uDC7C](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC86\uDC87\uDEB6](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDC83\uDD7A](?:\uD83C[\uDFFB-\uDFFF])?|\uDD74(?:(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F))?|\uDC6F(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|[\uDEA3\uDEB4\uDEB5](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDEC0\uDECC\uDC6D\uDC6B\uDC6C](?:\uD83C[\uDFFB-\uDFFF])?|\uDDE3\uFE0F?|\uDC15(?:\u200D\uD83E\uDDBA)?|[\uDC3F\uDD4A\uDD77\uDD78\uDDFA\uDEE3\uDEE4\uDEE2\uDEF3\uDEE5\uDEE9\uDEF0\uDECE\uDD70\uDD79\uDDBC\uDD76\uDECD\uDDA5\uDDA8\uDDB1\uDDB2\uDCFD\uDD6F\uDDDE\uDDF3\uDD8B\uDD8A\uDD8C\uDD8D\uDDC2\uDDD2\uDDD3\uDD87\uDDC3\uDDC4\uDDD1\uDDDD\uDEE0\uDDE1\uDEE1\uDDDC\uDECF\uDECB\uDD49]\uFE0F?|[\uDE00\uDE03\uDE04\uDE01\uDE06\uDE05\uDE02\uDE42\uDE43\uDE09\uDE0A\uDE07\uDE0D\uDE18\uDE17\uDE1A\uDE19\uDE0B\uDE1B-\uDE1D\uDE10\uDE11\uDE36\uDE0F\uDE12\uDE44\uDE2C\uDE0C\uDE14\uDE2A\uDE34\uDE37\uDE35\uDE0E\uDE15\uDE1F\uDE41\uDE2E\uDE2F\uDE32\uDE33\uDE26-\uDE28\uDE30\uDE25\uDE22\uDE2D\uDE31\uDE16\uDE23\uDE1E\uDE13\uDE29\uDE2B\uDE24\uDE21\uDE20\uDE08\uDC7F\uDC80\uDCA9\uDC79-\uDC7B\uDC7D\uDC7E\uDE3A\uDE38\uDE39\uDE3B-\uDE3D\uDE40\uDE3F\uDE3E\uDE48-\uDE4A\uDC8B\uDC8C\uDC98\uDC9D\uDC96\uDC97\uDC93\uDC9E\uDC95\uDC9F\uDC94\uDC9B\uDC9A\uDC99\uDC9C\uDDA4\uDCAF\uDCA2\uDCA5\uDCAB\uDCA6\uDCA8\uDCA3\uDCAC\uDCAD\uDCA4\uDC40\uDC45\uDC44\uDC8F\uDC91\uDC6A\uDC64\uDC65\uDC63\uDC35\uDC12\uDC36\uDC29\uDC3A\uDC31\uDC08\uDC2F\uDC05\uDC06\uDC34\uDC0E\uDC2E\uDC02-\uDC04\uDC37\uDC16\uDC17\uDC3D\uDC0F\uDC11\uDC10\uDC2A\uDC2B\uDC18\uDC2D\uDC01\uDC00\uDC39\uDC30\uDC07\uDC3B\uDC28\uDC3C\uDC3E\uDC14\uDC13\uDC23-\uDC27\uDC38\uDC0A\uDC22\uDC0D\uDC32\uDC09\uDC33\uDC0B\uDC2C\uDC1F-\uDC21\uDC19\uDC1A\uDC0C\uDC1B-\uDC1E\uDC90\uDCAE\uDD2A\uDDFE\uDDFB\uDC92\uDDFC\uDDFD\uDD4C\uDED5\uDD4D\uDD4B\uDC88\uDE82-\uDE8A\uDE9D\uDE9E\uDE8B-\uDE8E\uDE90-\uDE9C\uDEF5\uDEFA\uDEB2\uDEF4\uDEF9\uDE8F\uDEA8\uDEA5\uDEA6\uDED1\uDEA7\uDEF6\uDEA4\uDEA2\uDEEB\uDEEC\uDCBA\uDE81\uDE9F-\uDEA1\uDE80\uDEF8\uDD5B\uDD67\uDD50\uDD5C\uDD51\uDD5D\uDD52\uDD5E\uDD53\uDD5F\uDD54\uDD60\uDD55\uDD61\uDD56\uDD62\uDD57\uDD63\uDD58\uDD64\uDD59\uDD65\uDD5A\uDD66\uDD25\uDCA7\uDEF7\uDD2E\uDC53-\uDC62\uDC51\uDC52\uDCFF\uDC84\uDC8D\uDC8E\uDD07-\uDD0A\uDCE2\uDCE3\uDCEF\uDD14\uDD15\uDCFB\uDCF1\uDCF2\uDCDE-\uDCE0\uDD0B\uDD0C\uDCBB\uDCBD-\uDCC0\uDCFA\uDCF7-\uDCF9\uDCFC\uDD0D\uDD0E\uDCA1\uDD26\uDCD4-\uDCDA\uDCD3\uDCD2\uDCC3\uDCDC\uDCC4\uDCF0\uDCD1\uDD16\uDCB0\uDCB4-\uDCB8\uDCB3\uDCB9\uDCB1\uDCB2\uDCE7-\uDCE9\uDCE4-\uDCE6\uDCEB\uDCEA\uDCEC-\uDCEE\uDCDD\uDCBC\uDCC1\uDCC2\uDCC5-\uDCD0\uDD12\uDD13\uDD0F-\uDD11\uDD28\uDD2B\uDD27\uDD29\uDD17\uDD2C\uDD2D\uDCE1\uDC89\uDC8A\uDEAA\uDEBD\uDEBF\uDEC1\uDED2\uDEAC\uDDFF\uDEAE\uDEB0\uDEB9-\uDEBC\uDEBE\uDEC2-\uDEC5\uDEB8\uDEAB\uDEB3\uDEAD\uDEAF\uDEB1\uDEB7\uDCF5\uDD1E\uDD03\uDD04\uDD19-\uDD1D\uDED0\uDD4E\uDD2F\uDD00-\uDD02\uDD3C\uDD3D\uDD05\uDD06\uDCF6\uDCF3\uDCF4\uDD31\uDCDB\uDD30\uDD1F-\uDD24\uDD34\uDFE0-\uDFE2\uDD35\uDFE3-\uDFE5\uDFE7-\uDFE9\uDFE6\uDFEA\uDFEB\uDD36-\uDD3B\uDCA0\uDD18\uDD33\uDD32\uDEA9])|\uD83E(?:[\uDD1A\uDD0F\uDD1E\uDD1F\uDD18\uDD19\uDD1B\uDD1C\uDD32\uDD33\uDDB5\uDDB6\uDDBB\uDDD2](?:\uD83C[\uDFFB-\uDFFF])?|\uDDD1(?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:\uD83E(?:\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?))?)|\u200D(?:\uD83E(?:\uDD1D\u200D\uD83E\uDDD1|[\uDDB0\uDDB1\uDDB3\uDDB2\uDDAF\uDDBC\uDDBD])|\u2695\uFE0F?|\uD83C[\uDF93\uDFEB\uDF3E\uDF73\uDFED\uDFA4\uDFA8]|\u2696\uFE0F?|\uD83D[\uDD27\uDCBC\uDD2C\uDCBB\uDE80\uDE92]|\u2708\uFE0F?)))?|[\uDDD4\uDDD3](?:\uD83C[\uDFFB-\uDFFF])?|[\uDDCF\uDD26\uDD37](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDD34\uDDD5\uDD35\uDD30\uDD31\uDD36](?:\uD83C[\uDFFB-\uDFFF])?|[\uDDB8\uDDB9\uDDD9-\uDDDD](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDDDE\uDDDF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?|[\uDDCD\uDDCE\uDDD6\uDDD7\uDD38](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|\uDD3C(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|[\uDD3D\uDD3E\uDD39\uDDD8](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDD23\uDD70\uDD29\uDD2A\uDD11\uDD17\uDD2D\uDD2B\uDD14\uDD10\uDD28\uDD25\uDD24\uDD12\uDD15\uDD22\uDD2E\uDD27\uDD75\uDD76\uDD74\uDD2F\uDD20\uDD73\uDD13\uDDD0\uDD7A\uDD71\uDD2C\uDD21\uDD16\uDDE1\uDD0E\uDD0D\uDD1D\uDDBE\uDDBF\uDDE0\uDDB7\uDDB4\uDD3A\uDDB0\uDDB1\uDDB3\uDDB2\uDD8D\uDDA7\uDDAE\uDD8A\uDD9D\uDD81\uDD84\uDD93\uDD8C\uDD99\uDD92\uDD8F\uDD9B\uDD94\uDD87\uDDA5\uDDA6\uDDA8\uDD98\uDDA1\uDD83\uDD85\uDD86\uDDA2\uDD89\uDDA9\uDD9A\uDD9C\uDD8E\uDD95\uDD96\uDD88\uDD8B\uDD97\uDD82\uDD9F\uDDA0\uDD40\uDD6D\uDD5D\uDD65\uDD51\uDD54\uDD55\uDD52\uDD6C\uDD66\uDDC4\uDDC5\uDD5C\uDD50\uDD56\uDD68\uDD6F\uDD5E\uDDC7\uDDC0\uDD69\uDD53\uDD6A\uDD59\uDDC6\uDD5A\uDD58\uDD63\uDD57\uDDC8\uDDC2\uDD6B\uDD6E\uDD5F-\uDD61\uDD80\uDD9E\uDD90\uDD91\uDDAA\uDDC1\uDD67\uDD5B\uDD42\uDD43\uDD64\uDDC3\uDDC9\uDDCA\uDD62\uDD44\uDDED\uDDF1\uDDBD\uDDBC\uDE82\uDDF3\uDE90\uDDE8\uDDE7\uDD47-\uDD49\uDD4E\uDD4F\uDD4D\uDD4A\uDD4B\uDD45\uDD3F\uDD4C\uDE80\uDE81\uDDFF\uDDE9\uDDF8\uDDF5\uDDF6\uDD7D\uDD7C\uDDBA\uDDE3-\uDDE6\uDD7B\uDE71-\uDE73\uDD7E\uDD7F\uDE70\uDDE2\uDE95\uDD41\uDDEE\uDE94\uDDFE\uDE93\uDDAF\uDDF0\uDDF2\uDDEA-\uDDEC\uDE78-\uDE7A\uDE91\uDE92\uDDF4\uDDF7\uDDF9-\uDDFD\uDDEF])|[\u263A\u2639\u2620\u2763\u2764]\uFE0F?|\u270B(?:\uD83C[\uDFFB-\uDFFF])?|[\u270C\u261D](?:(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F))?|\u270A(?:\uD83C[\uDFFB-\uDFFF])?|\u270D(?:(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F))?|\uD83C(?:\uDF85(?:\uD83C[\uDFFB-\uDFFF])?|\uDFC3(?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDFC7\uDFC2](?:\uD83C[\uDFFB-\uDFFF])?|\uDFCC(?:(?:\uFE0F(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDFC4\uDFCA](?:(?:\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|\uDFCB(?:(?:\uFE0F(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\uDFF5\uDF36\uDF7D\uDFD4-\uDFD6\uDFDC-\uDFDF\uDFDB\uDFD7\uDFD8\uDFDA\uDFD9\uDFCE\uDFCD\uDF21\uDF24-\uDF2C\uDF97\uDF9F\uDF96\uDF99-\uDF9B\uDF9E\uDFF7\uDD70\uDD71\uDD7E\uDD7F\uDE02\uDE37]\uFE0F?|\uDFF4(?:(?:\u200D\u2620\uFE0F?|\uDB40\uDC67\uDB40\uDC62\uDB40(?:\uDC65\uDB40\uDC6E\uDB40\uDC67\uDB40\uDC7F|\uDC73\uDB40\uDC63\uDB40\uDC74\uDB40\uDC7F|\uDC77\uDB40\uDC6C\uDB40\uDC73\uDB40\uDC7F)))?|\uDFF3(?:(?:\uFE0F(?:\u200D\uD83C\uDF08)?|\u200D\uD83C\uDF08))?|\uDDE6\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF]|\uDDE7\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF]|\uDDE8\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF5\uDDF7\uDDFA-\uDDFF]|\uDDE9\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF]|\uDDEA\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA]|\uDDEB\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7]|\uDDEC\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE]|\uDDED\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA]|\uDDEE\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9]|\uDDEF\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5]|\uDDF0\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF]|\uDDF1\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE]|\uDDF2\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF]|\uDDF3\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF]|\uDDF4\uD83C\uDDF2|\uDDF5\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE]|\uDDF6\uD83C\uDDE6|\uDDF7\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC]|\uDDF8\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF]|\uDDF9\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF]|\uDDFA\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF]|\uDDFB\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA]|\uDDFC\uD83C[\uDDEB\uDDF8]|\uDDFD\uD83C\uDDF0|\uDDFE\uD83C[\uDDEA\uDDF9]|\uDDFF\uD83C[\uDDE6\uDDF2\uDDFC]|[\uDFFB-\uDFFF\uDF38-\uDF3C\uDF37\uDF31-\uDF35\uDF3E-\uDF43\uDF47-\uDF53\uDF45\uDF46\uDF3D\uDF44\uDF30\uDF5E\uDF56\uDF57\uDF54\uDF5F\uDF55\uDF2D-\uDF2F\uDF73\uDF72\uDF7F\uDF71\uDF58-\uDF5D\uDF60\uDF62-\uDF65\uDF61\uDF66-\uDF6A\uDF82\uDF70\uDF6B-\uDF6F\uDF7C\uDF75\uDF76\uDF7E\uDF77-\uDF7B\uDF74\uDFFA\uDF0D-\uDF10\uDF0B\uDFE0-\uDFE6\uDFE8-\uDFED\uDFEF\uDFF0\uDF01\uDF03-\uDF07\uDF09\uDFA0-\uDFA2\uDFAA\uDF11-\uDF20\uDF0C\uDF00\uDF08\uDF02\uDF0A\uDF83\uDF84\uDF86-\uDF8B\uDF8D-\uDF91\uDF80\uDF81\uDFAB\uDFC6\uDFC5\uDFC0\uDFD0\uDFC8\uDFC9\uDFBE\uDFB3\uDFCF\uDFD1-\uDFD3\uDFF8\uDFA3\uDFBD\uDFBF\uDFAF\uDFB1\uDFAE\uDFB0\uDFB2\uDCCF\uDC04\uDFB4\uDFAD\uDFA8\uDF92\uDFA9\uDF93\uDFBC\uDFB5\uDFB6\uDFA4\uDFA7\uDFB7-\uDFBB\uDFA5\uDFAC\uDFEE\uDFF9\uDFE7\uDFA6\uDD8E\uDD91-\uDD9A\uDE01\uDE36\uDE2F\uDE50\uDE39\uDE1A\uDE32\uDE51\uDE38\uDE34\uDE33\uDE3A\uDE35\uDFC1\uDF8C])|\u26F7\uFE0F?|\u26F9(?:(?:\uFE0F(?:\u200D(?:[\u2642\u2640]\uFE0F?))?|\uD83C(?:[\uDFFB-\uDFFF](?:\u200D(?:[\u2642\u2640]\uFE0F?))?)|\u200D(?:[\u2642\u2640]\uFE0F?)))?|[\u2618\u26F0\u26E9\u2668\u26F4\u2708\u23F1\u23F2\u2600\u2601\u26C8\u2602\u26F1\u2744\u2603\u2604\u26F8\u2660\u2665\u2666\u2663\u265F\u26D1\u260E\u2328\u2709\u270F\u2712\u2702\u26CF\u2692\u2694\u2699\u2696\u26D3\u2697\u26B0\u26B1\u26A0\u2622\u2623\u2B06\u2197\u27A1\u2198\u2B07\u2199\u2B05\u2196\u2195\u2194\u21A9\u21AA\u2934\u2935\u269B\u2721\u2638\u262F\u271D\u2626\u262A\u262E\u25B6\u23ED\u23EF\u25C0\u23EE\u23F8-\u23FA\u23CF\u2640\u2642\u2695\u267E\u267B\u269C\u2611\u2714\u2716\u303D\u2733\u2734\u2747\u203C\u2049\u3030\u00A9\u00AE\u2122]\uFE0F?|[\u0023\u002A\u0030-\u0039](?:\uFE0F\u20E3|\u20E3)|[\u2139\u24C2\u3297\u3299\u25FC\u25FB\u25AA\u25AB]\uFE0F?|[\u2615\u26EA\u26F2\u26FA\u26FD\u2693\u26F5\u231B\u23F3\u231A\u23F0\u2B50\u26C5\u2614\u26A1\u26C4\u2728\u26BD\u26BE\u26F3\u267F\u26D4\u2648-\u2653\u26CE\u23E9-\u23EC\u2B55\u2705\u274C\u274E\u2795-\u2797\u27B0\u27BF\u2753-\u2755\u2757\u26AB\u26AA\u2B1B\u2B1C\u25FE\u25FD])";
        //"\\([0-9A-Za-z]+)((\\|[0-9]+){0,2})(##[0-9a-f]{6})?(#[^=\\]]+)?(=[^\\]]+)?\\]"// [\\w*/]*? --路径匹配
        protected const string m_RegexTag = "\\{([0-9A-Za-z]+[-(0-9A-Za-z)]*)((\\|[0-9]+){0,2})(##[0-9a-fA-F]+)?(#[^=\\}]*)?(=[^\\}]*)?\\}";
        protected const string m_RegexCustom = "<material=u(#[0-9a-fA-F]+)?>(((?!</material>)[\\s\\S])*)</material>";// 下划线
        protected const string m_RegexRichFormat = "<.*?>";// 富文本
        protected const string m_RegexNumber = @"\d+(\.\d+)?";// quad size 、width
        protected readonly StringBuilder m_Builder = new StringBuilder();
        protected readonly StringBuilder m_UnicodeName = new StringBuilder();
        protected readonly Dictionary<int, EmojiInfo> m_Emojis = new Dictionary<int, EmojiInfo>();
        protected readonly Dictionary<int, EmojiInfo> m_EmojisReallyIndex = new Dictionary<int, EmojiInfo>();
        protected readonly Dictionary<string, List<GameObject>> m_GameObjects = new Dictionary<string, List<GameObject>>();
        protected readonly List<RectTransform> m_Rects = new List<RectTransform>();
        protected readonly List<Image> m_Images = new List<Image>();
        protected readonly List<HrefInfo> m_Hrefs = new List<HrefInfo>();
        protected readonly List<UnderlineInfo> m_Underlines = new List<UnderlineInfo>();
        protected readonly UIVertex[] m_TempVerts = new UIVertex[4];
        protected readonly MatchResult m_MatchResult = new MatchResult();
#if UNITY_EDITOR
        protected readonly List<GameObject> m_Effects = new List<GameObject>();
#endif
        protected Coroutine m_ImageCouroutine;
        public Vector2 UnderLineOffset = new Vector2(0.055f, 1.5f);
        public bool IsDrawUnderLine { get; set; } = true;
        public bool IsAutoLineFeed => cachedTextGenerator.characterCount * 4 == cachedTextGenerator.vertexCount;
        protected Dictionary<int, EmojiInfo> m_EmojisDic => IsAutoLineFeed ? m_Emojis : m_EmojisReallyIndex;

        #endregion

        #region -----------------------------------------> 资源读取 <------------------------------------------

        protected static Dictionary<string, Dictionary<string, SpriteInfo>> m_EmojiData = new Dictionary<string, Dictionary<string, SpriteInfo>>();
        protected Dictionary<string, SpriteInfo> EmojiData
        {
            get
            {
                if (m_EmojiData.TryGetValue(EmojiType, out var selectEmoji))
                    return selectEmoji;

                ReadEmojiConfig(ref selectEmoji);
                return selectEmoji;
            }
        }

        public bool AutoFillEmoji = false;
        public string EmojiType = "Default";//获取用户选择的Emoji名字（或者以后改为其他方式）
        protected string m_EmojiType;
        protected void ReadEmojiConfig(ref Dictionary<string, SpriteInfo> selectEmoji)
        {
            var config = EmojiConfig();
            if (config == null)
                return;

            if (selectEmoji == null)
                selectEmoji = new Dictionary<string, SpriteInfo>();

            string emojiContent = config.text;
            string[] lines = emojiContent.Split('\n');
            for (int i = 1, imax = lines.Length; i < imax; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    string[] strs = lines[i].Split('\t');
                    SpriteInfo info = new SpriteInfo();
                    info.frame = int.Parse(strs[1]);
                    info.index = int.Parse(strs[2]);
                    selectEmoji.Add(strs[0], info);
                }
            }
            m_EmojiData.Add(EmojiType, selectEmoji);
        }
        protected TextAsset EmojiConfig()
        {
            return LoadConfigAssets<TextAsset>(EmojiType.ToLower() + "_config");
        }
        protected Material EmojiMat()
        {
            return LoadConfigAssets<Material>(EmojiType.ToLower() + "_mat");
        }
        protected T LoadConfigAssets<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(EmojiType))
                return null;

            return LoadAssets<T>(string.Format("Emoji/{0}/{1}", EmojiType, path));
            //var type = EmojiType.IsEmpty() ? string.Empty : "/";
            //var loadPath = "Emoji/" + EmojiType + type + path;
            //return LoadAssets<T>(loadPath);
        }
        protected T LoadAssets<T>(string loadPath) where T : Object
        {
            // NOTE: 这里的加载路径需要根据项目进行调整
            //       目前的路径为 Resources
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                var extension = string.Empty;
                if (typeof(T).IsAssignableFrom(typeof(Material)))
                    extension = ".mat";
                else if (typeof(T).IsAssignableFrom(typeof(TextAsset)))
                    extension = ".txt";
                else if (typeof(T).IsAssignableFrom(typeof(Sprite)))
                    extension = ".png";

                loadPath = "Assets/Resources/" + loadPath + extension;
                return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(loadPath);
            }
#endif
            return Resources.Load<T>(loadPath);
        }

        #endregion

        #region -----------------------------------------> 外部回调 <------------------------------------------

        protected Action<Image, string> m_EmojiFillHandler;
        /// <summary>0x02 Fill Image，(Image, link) </summary>
        public void FillEmoji(Action<Image, string> callback, bool isClear = true)
        {
            if (isClear)
                m_EmojiFillHandler = callback;
            else
                m_EmojiFillHandler += callback;
        }
        protected Action<RectTransform, string> m_CustomFillHandler;
        /// <summary>0x03 Custom Fill (RectTransform, link)</summary>
        public void FillCustom(Action<RectTransform, string> callback, bool isClear = true)
        {
            if (isClear)
                m_CustomFillHandler = callback;
            else
                m_CustomFillHandler += callback;
        }
        protected Action<int, string> m_HyperlinkClickEvent;
        /// <summary>0x01 Hyper Link Click Event (boxIndex, url)</summary>
        public void SetHyperlinkListener(Action<int, string> callback, bool isClear = true)
        {
            if (isClear)
                m_HyperlinkClickEvent = callback;
            else
                m_HyperlinkClickEvent += callback;
        }
        protected Action m_OnPopulateMeshOverEvent;
        /// <summary>OnPopulateMesh Event</summary>
        public void SetOnPopulateMeshOverListener(Action callback, bool isClear = true)
        {
            if (isClear)
                m_OnPopulateMeshOverEvent = callback;
            else
                m_OnPopulateMeshOverEvent += callback;
        }
        protected Action<Vector2> m_PointerClickEvent;
        /// <summary>PointerClick Event (localPoint)</summary>
        public void SetPointerClickListener(Action<Vector2> callback, bool isClear = true)
        {
            if (isClear)
                m_PointerClickEvent = callback;
            else
                m_PointerClickEvent += callback;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
            m_PointerClickEvent?.Invoke(localPoint);
            for (int h = 0, hmax = m_Hrefs.Count; h < hmax; h++)
            {
                var hrefInfo = m_Hrefs[h];
                var boxes = hrefInfo.boxes;
                for (var i = 0; i < boxes.Count; ++i)
                {
                    if (boxes[i].Contains(localPoint))
                    {
                        if (m_HyperlinkClickEvent == null)
                            continue;

                        m_HyperlinkClickEvent(h, hrefInfo.url);
                        return;
                    }
                }
            }
        }

        #region 为了不能包更而做的妥协，暂存

        //----------------------------------------------- 功能回调函数 ------------------------------------------------------

        //private Action<Vector2> m_PointerClick;
        //public void SetPointerClickListener(Action<Vector2> callback)
        //{
        //    m_PointerClick = callback;
        //}
        //private Func<string> m_GetHyperText;
        ///// <summary> 获取 m_HyperOutputText 的回调</summary>
        //public void SetHyperTextListener(Func<string> callback)
        //{
        //    m_GetHyperText = callback;
        //}
        //private Action<string> m_SetHyperText;
        ///// <summary> 设置 m_HyperOutputText 的回调</summary>
        //public void SetHyperTextListener(Action<string> callback)
        //{
        //    m_SetHyperText = callback;
        //}
        //private Action<string, Material, int, Color> m_ParseText;
        ///// <summary> 解析原字符串 的回调</summary>
        //public void SetParseTextListener(Action<string, Material, int, Color> callback)
        //{
        //    m_ParseText = callback;
        //}
        //private Func<UIVertex, int, UIVertex> m_DealWithEmojiData;
        ///// <summary> 填充emoji表情信息 的回调</summary>
        //public void SetDealWithEmojiDataListener(Func<UIVertex, int, UIVertex> callback)
        //{
        //    m_DealWithEmojiData = callback;
        //}
        //private Action<VertexHelper> m_ComputeBounds;
        ///// <summary> 计算边界 的回调</summary>
        //public void SetComputeBoundsListener(Action<VertexHelper> callback)
        //{
        //    m_ComputeBounds = callback;
        //}
        //private Func<bool> m_IsDraw;
        ///// <summary> 是否有必要绘制 的回调</summary>
        //public void SetIsDrawUnderLineListener(Func<bool> callback)
        //{
        //    m_IsDraw = callback;
        //}
        //private Action<VertexHelper, UIVertex[], IList<UIVertex>> m_DrawUnderLine;
        ///// <summary> 绘制下划线 的回调</summary>
        //public void SetDrawUnderLineListener(Action<VertexHelper, UIVertex[], IList<UIVertex>> callback)
        //{
        //    m_DrawUnderLine = callback;
        //}
        //private Action m_ShowImages;
        ///// <summary> 解析原字符串 的回调</summary>
        //public void SetShowImagesListener(Action callback)
        //{
        //    m_ShowImages = callback;
        //}

        #endregion

        #endregion

        #region -----------------------------------------> 重写方法 <------------------------------------------

        public override string text
        {
            get { return m_Text; }
            set
            {
                //NOTE:如果是相同内容就直接跳过了，防止不刷新富文本计算
                m_Text = "\u00A0";
                //NOTE:在文本长或高为0时，获取不到正确的解析
                ParseText(value);
                base.text = value;
            }
        }
        public override float preferredWidth
        {
            get
            {
                var settings = GetGenerationSettings(Vector2.zero);
                return cachedTextGeneratorForLayout.GetPreferredWidth(m_HyperOutputText, settings) / pixelsPerUnit;
            }
        }
        public override float preferredHeight
        {
            get
            {
                var settings = GetGenerationSettings(new Vector2(rectTransform.rect.size.x, 0.0f));
                return cachedTextGeneratorForLayout.GetPreferredHeight(m_HyperOutputText, settings) / pixelsPerUnit;
            }
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null)
                return;

            // NOTE: 如果GameObject是关闭状态可以不更新
            if (!gameObject.activeSelf || string.IsNullOrEmpty(m_Text) ||
                !Regex.IsMatch(m_Text, m_RegexEmoji) &&
                (base.preferredHeight <= 0 || base.preferredWidth <= 0 ||
                rectTransform.rect.height <= 0 || rectTransform.rect.width <= 0))
            {
                toFill.Clear();
                ResetField();
                return;
            }

            //Debug.LogError($"[{GetInstanceID()}] OnPopulateMesh !!\n[{m_Text}]\n");
            // NOTE: 放这里显示会比较怪，先不用这种方法
            //ResetField();
            //CheckMaterial();
            ParseText(m_Text);

            // We don't care if we the font Texture changes while we are doing our Update.
            // The end result of cachedTextGenerator will be valid for this instance.
            // Otherwise we can get issues like Case 619238.
            m_DisableFontTextureRebuiltCallback = true;

            var extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.PopulateWithErrors(m_HyperOutputText, settings, gameObject);

            // Apply the offset to the vertices
            IList<UIVertex> verts = cachedTextGenerator.verts;//如果使用富文本，这里的verts数量是除去富文本格式之后的数量
            float unitsPerPixel = 1 / pixelsPerUnit;
            // Last 4 verts are always a new line... (\n) 不做处理，可能会少字符
            // NOTE: 2019.1.5f1及更高的版本中cachedTextGenerator的行数不同会造成富文本的verts数量不同，所以这个需要区别
            int vertCount = verts.Count;// - 4;

            // We have no verts to process just return (case 1037923)
            if (vertCount <= 0)
            {
                toFill.Clear();
                return;
            }

            Vector3 repairVec = new Vector3(0, fontSize * 0.1f);
            Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();
            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_TempVerts[tempVertsIndex] = verts[i];
                    m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
                    m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
            else
            {
                EmojiInfo info = null;
                Vector2 uv = Vector2.zero;
                var emojiDic = m_EmojisDic;
                for (int i = 0; i < vertCount; ++i)
                {
                    int index = i / 4;
                    int tempVertIndex = i & 3;
                    m_TempVerts[tempVertIndex] = verts[i];
                    m_TempVerts[tempVertIndex].position *= unitsPerPixel;
                    // 处理Emoji、Texture、Custom
                    if (emojiDic.TryGetValue(index, out info))
                    {
                        m_TempVerts[tempVertIndex].position -= repairVec;
                        if (info.type == MatchType.Emoji)
                        {
                            uv.x = info.sprite.index;
                            uv.y = info.sprite.frame;
                            m_TempVerts[tempVertIndex].uv0 += uv * 10;
                        }
                        else
                        {
                            if (tempVertIndex == 3)
                                info.texture.position = m_TempVerts[tempVertIndex].position;

                            m_TempVerts[tempVertIndex].position = m_TempVerts[0].position;
                        }
                    }
                    if (tempVertIndex == 3)
                        toFill.AddUIVertexQuad(m_TempVerts);
                }
                ComputeBoundsInfo(toFill);
                DrawUnderLine(toFill);
            }

            m_DisableFontTextureRebuiltCallback = false;

            if (gameObject.activeSelf && gameObject.activeInHierarchy)
            {
                if (m_ImageCouroutine != null)
                    StopCoroutine(m_ImageCouroutine);

                m_ImageCouroutine = StartCoroutine(ShowImages());
            }

            m_OnPopulateMeshOverEvent?.Invoke();
        }
        public override void Rebuild(CanvasUpdate update)
        {
            //StopCoroutine(m_ImageCouroutine);
            base.Rebuild(update);
        }
        protected override void OnDisable()
        {
            ResetField();
            base.OnDisable();
        }

        #endregion

        #region -----------------------------------------> 文本解析 <------------------------------------------

        protected void ParseText(string mText)
        {
            ResetField();
            CheckMaterial();

            mText = ParseLineBreak(mText);
            mText = ParseEmoji(mText);

            MatchCollection matches = Regex.Matches(mText, m_RegexTag);
            if (matches.Count > 0)
            {
                int textIndex = 0, imgIdx = 0, rectIdx = 0;
                for (int i = 0, imax = matches.Count; i < imax; i++)
                {
                    var match = matches[i];
                    m_MatchResult.Parse(match, fontSize);

                    switch (m_MatchResult.type)
                    {
                        //NOTE: 2019.1.5f1 及以后版本 对富文本的空占位符进行了优化，单行状态下verts数量是实际内容的数量
                        case MatchType.Emoji:
                            {
                                if (EmojiData == null)
                                    break;

                                var emojiName = DealWithUnicode(m_MatchResult.title, out var info);
                                if (info != null || EmojiData.TryGetValue(emojiName, out info))
                                {
                                    m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                    var temIndex = m_Builder.Length;
                                    var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                    m_Builder.Append("<quad size=");
                                    m_Builder.Append(m_MatchResult.height);
                                    m_Builder.Append(" width=");
                                    m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                    m_Builder.Append(" />");

                                    var emojiInfo = new EmojiInfo()
                                    {
                                        type = MatchType.Emoji,
                                        sprite = info,
                                        width = m_MatchResult.width,
                                        height = m_MatchResult.height
                                    };
                                    m_Emojis.Add(temIndex, emojiInfo);
                                    m_EmojisReallyIndex.Add(temReallyIndex, emojiInfo);

                                    if (m_MatchResult.HasUrl)
                                    {
                                        var hrefInfo = new HrefInfo()
                                        {
                                            show = false,
                                            startIndex = temIndex * 4,
                                            reallyStartIndex = temReallyIndex * 4,
                                            endIndex = temIndex * 4 + 3,
                                            reallyEndIndex = temReallyIndex * 4 + 3,
                                            url = m_MatchResult.url,
                                            color = m_MatchResult.GetColor(color)
                                        };
                                        m_Hrefs.Add(hrefInfo);
                                        m_Underlines.Add(hrefInfo);
                                    }

                                    textIndex = match.Index + match.Length;
                                }
                                break;
                            }
                        case MatchType.HyperLink:
                            {
                                //NOTE: Unity 2019.2.11f1 版本的富文本处理调整了在 OnPopulateMesh 之前进行
                                //      因此需要将富文本内容的 Index 去除这样点击区域和下划线的位置才是正确的
                                //      Unity 2017.3.1f1 版本已进行对比验证此问题
                                //      备注：在不同行数的时候引擎解析的长度会不同，所以Index是变化的，后面增加了Index的修正长度还是使用原长度
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                m_Builder.Append("<color=");
                                m_Builder.Append(m_MatchResult.GetHexColor(color));
                                m_Builder.Append(">");
                                var temIndex = m_Builder.Length;
                                var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                m_Builder.Append(m_MatchResult.link);
                                m_Builder.Append("</color>");

                                var href = new HrefInfo()
                                {
                                    show = true,
                                    //#if UNITY_2019_2_OR_NEWER
                                    startIndex = temIndex * 4,
                                    reallyStartIndex = temReallyIndex * 4,
                                    endIndex = (temIndex + m_MatchResult.link.Length) * 4 - 1,
                                    reallyEndIndex = (temReallyIndex + m_MatchResult.link.Length) * 4 - 1,
                                    url = m_MatchResult.url,
                                    color = m_MatchResult.GetColor(color),
                                };
                                m_Hrefs.Add(href);
                                m_Underlines.Add(href);

                                textIndex = match.Index + match.Length;
                                break;
                            }
                        case MatchType.CustomFill:
                        case MatchType.Texture:
                            {
                                m_Builder.Append(mText.Substring(textIndex, match.Index - textIndex));
                                var temIndex = m_Builder.Length;
                                var temReallyIndex = GetReallyIndex(m_Builder.ToString());
                                m_Builder.Append("<quad size=");
                                m_Builder.Append(m_MatchResult.height);
                                m_Builder.Append(" width=");
                                m_Builder.Append((m_MatchResult.width * 1.0f / m_MatchResult.height).ToString("f2"));
                                m_Builder.Append(" />");

                                var emojiInfo = new EmojiInfo()
                                {
                                    type = m_MatchResult.type,
                                    width = m_MatchResult.width,
                                    height = m_MatchResult.height,
                                    texture = new TextureInfo() { link = m_MatchResult.link, index = m_MatchResult.type == MatchType.Texture ? imgIdx++ : rectIdx++ }
                                };
                                m_Emojis.Add(temIndex, emojiInfo);
                                m_EmojisReallyIndex.Add(temReallyIndex, emojiInfo);

                                if (m_MatchResult.HasUrl)
                                {
                                    var hrefInfo = new HrefInfo()
                                    {
                                        show = false,
                                        startIndex = temIndex * 4,
                                        reallyStartIndex = temReallyIndex * 4,
                                        endIndex = temIndex * 4 + 3,
                                        reallyEndIndex = temReallyIndex * 4 + 3,
                                        url = m_MatchResult.url,
                                        color = m_MatchResult.GetColor(color)
                                    };
                                    m_Hrefs.Add(hrefInfo);
                                    m_Underlines.Add(hrefInfo);
                                }

                                textIndex = match.Index + match.Length;
                                break;
                            }
                    }
                }
                m_Builder.Append(mText.Substring(textIndex, mText.Length - textIndex));
                m_HyperOutputText = m_Builder.ToString();
            }
            else
            {
                m_HyperOutputText = mText;
            }

            matches = Regex.Matches(m_HyperOutputText, m_RegexCustom);
            for (int i = 0, imax = matches.Count; i < imax; i++)
            {
                var match = matches[i];
                if (match.Success && match.Groups.Count == 4)
                {
                    var htmlColor = match.Groups[1].Value;
                    //没有自定义alpha值就同步color的alpha
                    var htmlfontcolor = ColorUtility.ToHtmlStringRGBA(color);
                    var alpha = htmlColor.Length <= 7 ? htmlfontcolor.Substring(htmlfontcolor.Length - 2) : string.Empty;
                    if (!ColorUtility.TryParseHtmlString(htmlColor + alpha, out var lineColor))
                        lineColor = color;

                    var subMatch = match.Groups[2];
                    var temIndex = GetReallyIndex(m_HyperOutputText.Substring(0, subMatch.Index));
                    var underline = new UnderlineInfo()
                    {
                        show = true,
                        startIndex = subMatch.Index * 4,
                        reallyStartIndex = temIndex * 4,
                        endIndex = (subMatch.Index + subMatch.Length) * 4 - 1,
                        reallyEndIndex = (temIndex + subMatch.Length) * 4 - 1,
                        color = lineColor
                    };
                    m_Underlines.Add(underline);
                }
            }
        }
        protected void ResetField()
        {
            m_Builder.Length = 0;
            m_Emojis.Clear();
            m_EmojisReallyIndex.Clear();
            m_Hrefs.Clear();
            m_Underlines.Clear();
            ClearImages();
        }
        protected void CheckMaterial()
        {
            if (m_EmojiType == EmojiType)
                return;

            m_EmojiType = EmojiType;
            // 这里判断可根据项目需求进行拓展
            m_Material = EmojiMat();
        }
        protected int GetReallyIndex(string currentMatch)
        {
            var texture = 0;
            var total = currentMatch;
            //total = total.Replace(" ", string.Empty);
            var matchList = Regex.Matches(total, m_RegexRichFormat);
            for (int i = 0, imax = matchList.Count; i < imax; i++)
            {
                var match = matchList[i];
                if (!match.Success)
                    continue;

                //NOTE: 多个图片时的宽度要加入到实际长度中 - 每个quad有4个顶点
                if (match.Value.Contains("quad size="))
                {
                    //Debug.LogError(match.Value);
                    //var sizeMatchList = Regex.Matches(match.Value, m_RegexNumber);
                    //for (int j = 0, jmax = sizeMatchList.Count; j < jmax; j++)
                    //{
                    //    var sizeMatch = sizeMatchList[j];
                    //    if (!sizeMatch.Success)
                    //        continue;

                    //    Debug.LogError(sizeMatch.Value);
                    //    texture += float.Parse(sizeMatch.Value);
                    //}
                    texture += 1;
                }

                total = total.Replace(match.Value, string.Empty);
            }
            //回车不计算顶点
            total = total.Replace("\n", string.Empty);
            return total.Length + texture;
        }
        protected void ComputeBoundsInfo(VertexHelper toFill)
        {
            //Debug.LogError($"Line Num: [{cachedTextGenerator.lineCount}] Vertex Num: [{cachedTextGenerator.vertexCount}] CharacterCount = [{cachedTextGenerator.characterCount}]\n");
            UIVertex vert = new UIVertex();
            UIVertex underlinevert = new UIVertex();
            for (int u = 0, umax = m_Underlines.Count; u < umax; u++)
            {
                var underline = m_Underlines[u];
                underline.boxes.Clear();
                underline.charswidth.Clear();
#if UNITY_2019_2_OR_NEWER
                underline.CorrectionIndex(IsAutoLineFeed);
#endif
                if (underline.startIndex >= toFill.currentVertCount)
                    continue;

                //Debug.LogError($"---------------------  [{u}/{umax}]  ---------------------\nstartIndex--> [{underline.startIndex}]\t\tendIndex --> [{underline.endIndex}]\n");
                // Add hyper text vector index to bounds
                toFill.PopulateUIVertex(ref vert, underline.startIndex);
                var pos = vert.position;
                var lowest = pos.y;
                var charInfo = new CharVertsInfo();
                var bounds = new Bounds(pos, Vector3.zero);
                var lastLineNum = GetLineNum(underline.startIndex);
                for (int i = underline.startIndex, m = underline.endIndex; i <= m; i++)
                {
                    if (i >= toFill.currentVertCount)
                        break;

                    toFill.PopulateUIVertex(ref vert, i);
                    pos = vert.position;
                    if (lowest > pos.y)
                        lowest = pos.y;

                    //找到当前行的开始下标，一段下划线可能有多行，这里需要检测一下当前字符下标所在行数
                    var currentLineNum = GetLineNum(i);
                    //WARNING: float point is unsafety
                    if (pos.y < bounds.min.y && currentLineNum > lastLineNum)
                    {
                        //if in different lines
                        lastLineNum = currentLineNum;
                        underline.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(pos, Vector3.zero);
                        charInfo.SortLowestPos();
                        underline.charswidth.Add(charInfo);
                        charInfo = new CharVertsInfo();
                    }
                    else
                    {
                        //expand bounds
                        bounds.Encapsulate(pos);
                        if (i % 4 == 3)
                        {
                            var index = i + 4 > m || i + 4 >= toFill.currentVertCount ? i - 1 : i + 4;
                            toFill.PopulateUIVertex(ref underlinevert, index);
                            currentLineNum = GetLineNum(index);
                            if (underlinevert.position.y < bounds.min.y && currentLineNum > lastLineNum)
                                toFill.PopulateUIVertex(ref underlinevert, i - 1);

                            if (lowest > underlinevert.position.y)
                                lowest = underlinevert.position.y;

                            charInfo.charswidth.Add(underlinevert.position.x - pos.x);
                            charInfo.lowestPoslist.Add(new Vector2(pos.x, lowest));
                        }
                    }
                }
                //add bound
                underline.boxes.Add(new Rect(bounds.min, bounds.size));
                charInfo.SortLowestPos();
                underline.charswidth.Add(charInfo);
            }
        }
        protected void DrawUnderLine(VertexHelper toFill)
        {
            if (!IsDrawUnderLine)
                return;

            if (m_Underlines.Count <= 0)
                return;

            var extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate("_", settings);

            IList<UIVertex> uList = cachedTextGenerator.verts;
            if (uList.Count < 4)
                return;

            float startlimit = 0, endlimit = 0;
            float w_offset = Math.Max(UnderLineOffset.x, -1);// 0.055f;
            float h_offset = UnderLineOffset.y;// 1.5f;
            float h = uList[2].position.y - uList[1].position.y;
            Vector3[] temVecs = new Vector3[4];
            for (int i = 0, imax = m_Underlines.Count; i < imax; i++)
            {
                var info = m_Underlines[i];
                if (!info.show)
                    continue;

                for (int j = 0, jmax = info.boxes.Count; j < jmax; j++)
                {
                    var box = info.boxes[j];
                    if (box.width <= 0 || box.height <= 0)
                        continue;

                    // NOTE: 如果使用一个下划线，文本太长时两端会被拉伸到透明

                    // 每个字单独计算下划线
                    startlimit = 0;
                    endlimit = 0;
                    var widthinfo = info.charswidth[j];
                    var omax = widthinfo.charswidth.Count;
                    if (widthinfo.charswidth != null && widthinfo.charswidth.Count > 0)
                    {
                        startlimit = widthinfo.lowestPoslist[0].x;
                        endlimit = widthinfo.lowestPoslist[omax - 1].x + widthinfo.charswidth[omax - 1];
                    }
                    for (int o = 0; o < omax; o++)
                    {
                        var width = widthinfo.charswidth[o];
                        var minPos = widthinfo.lowestPoslist[o];
                        temVecs[0] = minPos + new Vector2(width * (0 - w_offset), h_offset + 0);
                        temVecs[1] = minPos + new Vector2(width * (1 + w_offset), h_offset + 0);
                        temVecs[2] = minPos + new Vector2(width * (1 + w_offset), h_offset + h);
                        temVecs[3] = minPos + new Vector2(width * (0 - w_offset), h_offset + h);

                        temVecs[0].x = Math.Max(temVecs[0].x, startlimit);
                        temVecs[1].x = Math.Min(temVecs[1].x, endlimit);
                        temVecs[2].x = Math.Min(temVecs[2].x, endlimit);
                        temVecs[3].x = Math.Max(temVecs[3].x, startlimit);

                        for (int k = 0; k < 4; k++)
                        {
                            m_TempVerts[k] = uList[k];
                            m_TempVerts[k].color = info.color;
                            m_TempVerts[k].position = temVecs[k];
                        }

                        toFill.AddUIVertexQuad(m_TempVerts);
                    }

                    // 平均计算的下划线，效果两端与字体不平齐
                    //var basePos = box.min;
                    //var popWidth = (info.endIndex - info.startIndex) / 4f;
                    //var w = box.width / popWidth;
                    //for (int p = 0; p < popWidth; p++)
                    //{
                    //    temVecs[0] = basePos + new Vector2(w * (0 - w_offset), h_offset + 0);
                    //    temVecs[1] = basePos + new Vector2(w * (1 + w_offset), h_offset + 0);
                    //    temVecs[2] = basePos + new Vector2(w * (1 + w_offset), h_offset + h);
                    //    temVecs[3] = basePos + new Vector2(w * (0 - w_offset), h_offset + h);

                    //    temVecs[0].x = Math.Max(temVecs[0].x, startlimit);
                    //    temVecs[1].x = Math.Min(temVecs[1].x, endlimit);
                    //    temVecs[2].x = Math.Min(temVecs[2].x, endlimit);
                    //    temVecs[3].x = Math.Max(temVecs[3].x, startlimit);

                    //    for (int k = 0; k < 4; k++)
                    //    {
                    //        m_TempVerts[k] = uList[k];
                    //        m_TempVerts[k].color = info.color;
                    //        m_TempVerts[k].position = temVecs[k];
                    //    }

                    //    toFill.AddUIVertexQuad(m_TempVerts);
                    //    basePos += new Vector2(w, 0);
                    //}
                }
            }
        }
        protected int GetLineNum(int currentIndex)
        {
            if (IsAutoLineFeed)
                currentIndex /= 4;

            int lineNum = 0;
            for (int l = 0, lmax = cachedTextGenerator.lineCount; l < lmax; l++)
            {
                if (currentIndex >= cachedTextGenerator.lines[l].startCharIdx)
                    lineNum = l;
                else
                    break;
            }
            return lineNum;
        }

        protected string ParseEmoji(string mText)
        {
            return Regex.Replace(mText, m_RegexEmoji, match =>
            {
                var emojiString = match.Value;
                m_UnicodeName.Length = 0;
                m_UnicodeName.Append("{");
                for (int i = 0, imax = emojiString.Length; i < imax; i += char.IsSurrogatePair(emojiString, i) ? 2 : 1)
                {
                    m_UnicodeName.AppendFormat("{0:x4}", char.ConvertToUtf32(emojiString, i));
                    var add = char.IsSurrogatePair(emojiString, i) ? 2 : 1;
                    if (i + add < imax)
                        m_UnicodeName.Append("-");
                }
                m_UnicodeName.Append("}");
                return m_UnicodeName.ToString();
            });
        }
        protected string DealWithUnicode(string emojiName, out SpriteInfo info)
        {
            if (!EmojiData.TryGetValue(emojiName, out info))
            {
                // 如果连接符导致找不到对应的Emoji就移除 末尾连接符
                var graghConnector = "fe0f";
                if (emojiName.EndsWith(graghConnector))
                    emojiName = emojiName.Remove(emojiName.Length - graghConnector.Length - 1);

                var textConnector = "fe0e";
                if (emojiName.EndsWith(textConnector))
                    emojiName = emojiName.Remove(emojiName.Length - textConnector.Length - 1);

                if (!EmojiData.TryGetValue(emojiName, out info))
                {
                    // 如果连接符导致找不到对应的Emoji就移除 所有连接符
                    if (emojiName.Contains(graghConnector))
                        emojiName = emojiName.Replace("-fe0f", string.Empty);

                    if (emojiName.Contains(textConnector))
                        emojiName = emojiName.Replace("-fe0e", string.Empty);

                    if (!EmojiData.TryGetValue(emojiName, out info))
                    {
                        // 如果连接符导致找不到对应的Emoji就 简略十进制显示位数
                        var omit = 0;
                        var unicodeArray = emojiName.Split('-');
                        for (int j = 0, jmax = unicodeArray.Length; j < jmax; j++)
                        {
                            //NOTE: 只检测第一个 ，这样会有潜在问题，现在的emoji 13 是没问题的
                            var unicode = unicodeArray[j];
                            //纯数字
                            if (int.TryParse(unicode, out omit))
                            {
                                emojiName = emojiName.Replace(unicode, omit.ToString());
                                break;
                            }
                            //数字字母混合，数字在前
                            if (Regex.IsMatch(unicode, m_RegexNumber))
                            {
                                var replace = string.Empty;
                                var unicodeMatches = Regex.Matches(unicode, m_RegexNumber);
                                for (int k = 0, kmax = unicodeMatches.Count; k < kmax; k++)
                                {
                                    if (int.TryParse(unicodeMatches[k].Value, out omit))
                                    {
                                        replace = unicode.Replace(unicodeMatches[k].Value, omit == 0 ? string.Empty : omit.ToString());
                                        break;
                                    }
                                }
                                emojiName = emojiName.Replace(unicode, replace);
                                break;
                            }
                        }
                    }
                }
            }
            return emojiName;
        }

        protected string ParseLineBreak(string mText)
        {
            // Non-Breaking Space
            return mText.Replace(" ", "\u00A0");
        }

        protected void ClearImages()
        {
            for (int i = 0, imax = m_Images.Count; i < imax; i++)
                m_Images[i].rectTransform.localScale = Vector3.zero;

            for (int i = 0, imax = m_Rects.Count; i < imax; i++)
                m_Rects[i].localScale = Vector3.zero;
        }
        protected IEnumerator ShowImages()
        {
            yield return null;
            var emojiList = m_EmojisDic.Values;
            foreach (var emojiInfo in emojiList)
            {
                if (emojiInfo.type == MatchType.Texture)
                {
                    emojiInfo.texture.image = GetImage(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor && !Application.isPlaying)
                    {
                        emojiInfo.texture.image.sprite = LoadAssets<Sprite>(emojiInfo.texture.link);
                        continue;
                    }
#endif
                    if (AutoFillEmoji)
                        emojiInfo.texture.image.sprite = LoadAssets<Sprite>(emojiInfo.texture.link);
                    else
                        m_EmojiFillHandler?.Invoke(emojiInfo.texture.image, emojiInfo.texture.link);
                }
                else if (emojiInfo.type == MatchType.CustomFill)
                {
                    emojiInfo.texture.rect = GetRectTransform(emojiInfo.texture, emojiInfo.width, emojiInfo.height);
#if UNITY_EDITOR
                    if (Application.isEditor && !Application.isPlaying)
                    {
                        GameObject obj = null;
                        var index = emojiInfo.texture.index;
                        if (m_Effects.Count > index)
                            obj = m_Effects[index];

                        if (obj == null)
                        {
                            var assets = LoadAssets<GameObject>(emojiInfo.texture.link);
                            if (assets == null)
                                continue;

                            obj = GameObject.Instantiate(assets);

                            if (m_Effects.Count > index)
                                m_Effects[index] = obj;
                            else
                                m_Effects.Add(obj);
                        }
                        var objRect = obj.transform as RectTransform;
                        objRect.SetParent(emojiInfo.texture.rect);
                        objRect.localScale = Vector3.one;
                        objRect.anchoredPosition = Vector2.zero;
                        continue;
                    }
#endif
                    m_CustomFillHandler?.Invoke(emojiInfo.texture.rect, emojiInfo.texture.link);
                }
            }
        }
        protected Image GetImage(TextureInfo info, int width, int height)
        {
            Image img = null;
            if (m_Images.Count > info.index)
                img = m_Images[info.index];

            if (img == null)
            {
                img = GetOrAddComponent<Image>("emoji_", info.index);
                img.transform.SetParent(transform);
                img.rectTransform.anchorMax = rectTransform.anchorMax;
                img.rectTransform.anchorMin = rectTransform.anchorMin;
                img.rectTransform.pivot = Vector2.zero;
                img.raycastTarget = false;

                if (m_Images.Count > info.index)
                    m_Images[info.index] = img;
                else
                    m_Images.Add(img);
            }

            img.rectTransform.localScale = info.position == Vector3.zero ? Vector3.zero : Vector3.one;
            img.rectTransform.sizeDelta = new Vector2(width, height);
            img.rectTransform.anchoredPosition3D = info.position;
            return img;
        }
        protected RectTransform GetRectTransform(TextureInfo info, int width, int height)
        {
            RectTransform rect = null;
            if (m_Rects.Count > info.index)
                rect = m_Rects[info.index];

            if (rect == null)
            {
                rect = GetOrAddComponent<RectTransform>("custom_", info.index);
                rect.SetParent(transform);
                rect.anchorMax = rectTransform.anchorMax;
                rect.anchorMin = rectTransform.anchorMin;
                rect.pivot = Vector2.zero;

                if (m_Rects.Count > info.index)
                    m_Rects[info.index] = rect;
                else
                    m_Rects.Add(rect);
            }
            rect.localScale = info.position == Vector3.zero ? Vector3.zero : Vector3.one;
            rect.sizeDelta = new Vector2(width, height);
            rect.anchoredPosition3D = info.position;
            return rect;
        }
        protected T GetOrAddComponent<T>(string name, int index) where T : Component
        {
            var obj = GetGameObject(name, index);
            var com = obj.GetComponent<T>();
            if (com == null)
                com = obj.AddComponent<T>();

            return com;
        }
        protected GameObject GetGameObject(string name, int index)
        {
            var key = name + index;
            var list = new List<GameObject>();
            if (m_GameObjects.ContainsKey(key))
                list = m_GameObjects[key];
            else
                m_GameObjects.Add(key, list);

            GameObject go = null;
            if (list.Count > index)
                go = list[index];

            if (go == null)
            {
                var child = transform.Find(key);
                if (child == null)
                    go = new GameObject(key);
                else
                    go = child.gameObject;

                go.layer = gameObject.layer;
                if (list.Count > index)
                    list[index] = go;
                else
                    list.Add(go);
            }
            return go;
        }

        #endregion

        #region -----------------------------------------> 构造器类 <------------------------------------------

        protected class SpriteInfo
        {
            public int index;
            public int frame;
        }
        protected class TextureInfo
        {
            public int index;
            public Image image;
            public RectTransform rect;
            public Vector3 position;
            public string link;
        }
        protected class EmojiInfo
        {
            public MatchType type;
            public int width;
            public int height;
            public SpriteInfo sprite;
            public TextureInfo texture;
        }
        protected enum MatchType
        {
            None,
            Emoji,
            HyperLink,
            Texture,
            CustomFill,
        }
        protected class MatchResult
        {
            public MatchType type;
            public string title;
            public string url;
            public string link;
            public int height;
            public int width;
            private string htmlColor;
            private Color color;

            void Reset()
            {
                type = MatchType.None;
                title = String.Empty;
                width = 0;
                height = 0;
                htmlColor = string.Empty;
                url = string.Empty;
                link = string.Empty;
            }
            public void Parse(Match match, int fontSize)
            {
                Reset();
                if (!match.Success || match.Groups.Count != 7)
                    return;

                title = match.Groups[1].Value;
                if (match.Groups[2].Success)
                {
                    string sizeStr = match.Groups[2].Value;
                    string[] size = sizeStr.Split('|');
                    height = size.Length > 1 ? int.Parse(size[1]) : fontSize;
                    width = size.Length == 3 ? int.Parse(size[2]) : height;
                }
                else
                {
                    height = fontSize;
                    width = fontSize;
                }
                if (match.Groups[4].Success)
                {
                    htmlColor = match.Groups[4].Value.Substring(1);
                }
                if (match.Groups[5].Success)
                {
                    url = match.Groups[5].Value.Substring(1);
                }
                if (match.Groups[6].Success)
                {
                    link = match.Groups[6].Value.Substring(1);
                }

                if (title.Equals("0x01"))
                {
                    // url 是否为空 不影响超链接的判定
                    if (!string.IsNullOrEmpty(link))
                        type = MatchType.HyperLink;
                }
                else if (title.Equals("0x02"))
                {
                    if (!string.IsNullOrEmpty(link))
                        type = MatchType.Texture;
                }
                else if (title.Equals("0x03"))
                {
                    if (!string.IsNullOrEmpty(link))
                        type = MatchType.CustomFill;
                }

                if (type == MatchType.None)
                    type = MatchType.Emoji;
            }
            public bool HasUrl { get { return !string.IsNullOrEmpty(url); } }
            public Color GetColor(Color fontColor)
            {
                if (string.IsNullOrEmpty(htmlColor))
                    return fontColor;

                //将color的alpha值更新到下划线
                HasAlpha(fontColor, out var htmlfontcolor, out var alpha);
                if (!ColorUtility.TryParseHtmlString(htmlColor + alpha, out color))
                    color = fontColor;

                return color;
            }
            public string GetHexColor(Color fontColor)
            {
                //将color的alpha值更新到超链接
                HasAlpha(fontColor, out var htmlfontcolor, out var alpha);
                if (!string.IsNullOrEmpty(htmlColor))
                    return htmlColor + alpha;

                return "#" + htmlfontcolor;
            }
            private bool HasAlpha(Color fontColor, out string htmlfontcolor, out string alpha)
            {
                alpha = string.Empty;
                htmlfontcolor = ColorUtility.ToHtmlStringRGBA(fontColor);
                //没有自定义alpha值就同步color的alpha
                if (htmlColor.Length <= 7)
                {
                    alpha = htmlfontcolor.Substring(htmlfontcolor.Length - 2);
                    return false;
                }
                return true;
            }
        }
        protected class UnderlineInfo
        {
            public bool show;
            public int startIndex;//包含富文本的index
            public int reallyStartIndex;//祛除富文本的index
            public int endIndex;//包含富文本的index
            public int reallyEndIndex;//祛除富文本的index
            public Color color;
            public readonly List<Rect> boxes = new List<Rect>();
            public readonly List<CharVertsInfo> charswidth = new List<CharVertsInfo>();

            public void CorrectionIndex(bool isMultiLine)
            {
                if (isMultiLine)
                {

                }
                else
                {
                    startIndex = reallyStartIndex;
                    endIndex = reallyEndIndex;
                }
            }
        }
        protected class HrefInfo : UnderlineInfo
        {
            public string url;
        }
        protected class CharVertsInfo
        {
            public List<float> charswidth = new List<float>();
            public List<Vector2> lowestPoslist = new List<Vector2>();

            public void SortLowestPos()
            {
                if (lowestPoslist.Count == 0)
                    return;

                var lowestPoslistCount = lowestPoslist.Count;
                var lowest = lowestPoslist[lowestPoslistCount - 1].y;
                for (int i = 0; i < lowestPoslistCount; i++)
                {
                    var vec = lowestPoslist[i];
                    if (vec.y == lowest)
                        break;

                    lowestPoslist[i] = new Vector2(vec.x, lowest);
                }
            }
        }

        #endregion

    }
}
