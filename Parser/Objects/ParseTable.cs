using System;
using System.Collections.Generic;
using Lexer.Objects;
using static Lexer.Objects.TokenType;

namespace Parser.Objects
{
    /// <summary>
    /// The parse table of the parser. This is where the parse rules are set.
    /// </summary>
    public class ParseTable
    {
        /// <summary>
        /// This is the table of the parser. This is where all transition rules are stored.
        /// </summary>
        /// <value></value>
        public Dictionary<TokenType, Dictionary<TokenType, ParseAction>> Table { get; private set; }

        /// <summary>
        /// Access of table using scanner tokens
        /// </summary>
        /// <value>A transition rule. <see cref="ParseAction"/></value>
        public ParseAction this[ScannerToken key1, ScannerToken key2]
        {
            get => Table[key1.Type][key2.Type];
            set => Table[key1.Type][key2.Type] = value;
        }
        /// <summary>
        /// Access of table using tokentypes
        /// </summary>
        /// <value>A transition rule. <see cref="ParseAction"/></value>
        public ParseAction this[TokenType key1, TokenType key2]
        {
            get => Table[key1][key2];
            set => Table[key1][key2] = value;
        }

        /// <summary>
        /// The constructor for the parse table. Here the table is initialised and created.
        /// </summary>
        public ParseTable()
        {
            Init();
            InitTable();
        }

        /// <summary>
        /// This method will initialise all locations as errors
        /// </summary>
        private void Init()
        {

            Table = new Dictionary<TokenType, Dictionary<TokenType, ParseAction>>();

            foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
            {
                Table.Add(type, new Dictionary<TokenType, ParseAction>());
                foreach (TokenType innerType in Enum.GetValues(typeof(TokenType)))
                {
                    Table[type].Add(innerType, ParseAction.Error());
                }
            }
        }
        
        /// <summary>
        /// This method fills in all the transition rules in the table.
        /// </summary>
        public void InitTable()
        {
            this[PROG, EOF] = new ParseAction(0,STMNTS);
            this[PROG, WAIT] = new ParseAction(1,STMNTS);
            this[PROG, VAR] = new ParseAction(2,STMNTS);
            this[PROG, BEGIN] = new ParseAction(3,STMNTS);
            this[PROG, FUNC] = new ParseAction(4,STMNTS);
            this[PROG, CALL] = new ParseAction(5,STMNTS);
            this[PROG, IF] = new ParseAction(6,STMNTS);
            this[PROG, APIN] = new ParseAction(7,STMNTS);
            this[PROG, DPIN] = new ParseAction(8,STMNTS);
            this[PROG, COMMENT] = new ParseAction(9,NT_COMMENT);
            this[PROG, MULT_COMNT] = new ParseAction(10,NT_COMMENT);

            this[NT_COMMENT, COMMENT] = new ParseAction(11,COMMENT);
            this[NT_COMMENT, MULT_COMNT] = new ParseAction(12,MULT_COMNT);

            this[STMNTS, EOF] = new ParseAction(0);
            this[STMNTS, WAIT] = new ParseAction(13,STMNT, STMNTS);
            this[STMNTS, END] = new ParseAction(0);
            this[STMNTS, VAR] = new ParseAction(14,STMNT, STMNTS);
            this[STMNTS, ARRAYINDEX] = new ParseAction(201,STMNT, STMNTS);
            this[STMNTS, BEGIN] = new ParseAction(15,STMNT, STMNTS);
            this[STMNTS, RETURN] = new ParseAction(0);
            this[STMNTS, FUNC] = new ParseAction(16,FUNCDECL, STMNTS);
            this[STMNTS, CALL] = new ParseAction(17,STMNT, STMNTS);
            this[STMNTS, IF] = new ParseAction(18,STMNT, STMNTS);
            this[STMNTS, ELSE] = new ParseAction(0);
            this[STMNTS, APIN] = new ParseAction(19,STMNT, STMNTS);
            this[STMNTS, DPIN] = new ParseAction(20,STMNT, STMNTS);
            this[STMNTS, COMMENT] = new ParseAction(21,NT_COMMENT, STMNTS);
            this[STMNTS, MULT_COMNT] = new ParseAction(22,NT_COMMENT, STMNTS);

            this[STMNT, WAIT] = new ParseAction(23,WAITSTMNT);
            this[STMNT, VAR] = new ParseAction(24,ASSIGNSTMNT);
            this[STMNT, ARRAYINDEX] = new ParseAction(202,ASSIGNSTMNT);
            this[STMNT, BEGIN] = new ParseAction(25,BEGINSTMNT);
            this[STMNT, CALL] = new ParseAction(26,FUNCCALL);
            this[STMNT, IF] = new ParseAction(27,IFSTMNT);
            this[STMNT, APIN] = new ParseAction(28,ASSIGNSTMNT);
            this[STMNT, DPIN] = new ParseAction(29,ASSIGNSTMNT);

            this[ASSIGNSTMNT, VAR] = new ParseAction(30,VAR, ASSIGN, ASSIGNMENT);
            this[ASSIGNSTMNT, ARRAYINDEX] = new ParseAction(203,ARRAYACCESSING, ASSIGN, ASSIGNMENT);
            this[ASSIGNSTMNT, APIN] = new ParseAction(31,APIN, ASSIGN, ASSIGNMENT);
            this[ASSIGNSTMNT, DPIN] = new ParseAction(32,DPIN, ASSIGN, ASSIGNMENT);

            this[ASSIGNMENT, NUMERIC] = new ParseAction(33,EXPR);
            this[ASSIGNMENT, VAR] = new ParseAction(34,EXPR);
            this[ASSIGNMENT, CALL] = new ParseAction(35,FUNCCALL);
            this[ASSIGNMENT, APIN] = new ParseAction(36,EXPR);
            this[ASSIGNMENT, DPIN] = new ParseAction(37,EXPR);
            this[ASSIGNMENT, ARRAYLEFT] = new ParseAction(38,ARR);
            this[ASSIGNMENT, STRING] = new ParseAction(39,EXPR);
            this[ASSIGNMENT, OP_MINUS] = new ParseAction(40,EXPR);
            this[ASSIGNMENT, OP_LPAREN] = new ParseAction(41,EXPR);
            this[ASSIGNMENT, BOOL] = new ParseAction(136,EXPR);
            this[ASSIGNMENT, ARRAYINDEX] = new ParseAction(211,EXPR);

            this[EXPR, NUMERIC] = new ParseAction(42,TERM, FOLLOWTERM);
            this[EXPR, VAR] = new ParseAction(43,TERM, FOLLOWTERM);
            this[EXPR, ARRAYINDEX] = new ParseAction(212,TERM, FOLLOWTERM);
            this[EXPR, APIN] = new ParseAction(44,TERM, FOLLOWTERM);
            this[EXPR, DPIN] = new ParseAction(45,TERM, FOLLOWTERM);
            this[EXPR, STRING] = new ParseAction(46,TERM, FOLLOWTERM);
            this[EXPR, OP_MINUS] = new ParseAction(47,TERM, FOLLOWTERM);
            this[EXPR, OP_LPAREN] = new ParseAction(48,TERM, FOLLOWTERM);
            this[EXPR, BOOL] = new ParseAction(137,TERM, FOLLOWTERM);
            this[EXPR, CALL] = new ParseAction(200,FUNCCALL, FOLLOWTERM);

            this[FOLLOWTERM, EOF] = new ParseAction(9000);
            this[FOLLOWTERM, ARRAYINDEX] = new ParseAction(0);
            this[FOLLOWTERM, WAIT] = new ParseAction(9001);
            this[FOLLOWTERM, END] = new ParseAction(9002);
            this[FOLLOWTERM, DO] = new ParseAction(9003);
            this[FOLLOWTERM, VAR] = new ParseAction(9004);
            this[FOLLOWTERM, BEGIN] = new ParseAction(9005);
            this[FOLLOWTERM, RETURN] = new ParseAction(9006);
            this[FOLLOWTERM, FUNC] = new ParseAction(9007);
            this[FOLLOWTERM, CALL] = new ParseAction(9008);
            this[FOLLOWTERM, IF] = new ParseAction(9009);
            this[FOLLOWTERM, ELSE] = new ParseAction(90010);
            this[FOLLOWTERM, APIN] = new ParseAction(90011);
            this[FOLLOWTERM, DPIN] = new ParseAction(90012);
            this[FOLLOWTERM, DPIN] = new ParseAction(90013);
            this[FOLLOWTERM, OP_MINUS] = new ParseAction(49,TERMOP, EXPR);
            this[FOLLOWTERM, OP_EQUAL] = new ParseAction(50,TERMOP, EXPR);
            this[FOLLOWTERM, OP_OR] = new ParseAction(51,TERMOP, EXPR);
            this[FOLLOWTERM, OP_LESS] = new ParseAction(52,TERMOP, EXPR);
            this[FOLLOWTERM, OP_GREATER] = new ParseAction(53,TERMOP, EXPR);
            this[FOLLOWTERM, OP_AND] = new ParseAction(54,TERMOP, EXPR);
            this[FOLLOWTERM, OP_PLUS] = new ParseAction(55,TERMOP, EXPR);
            this[FOLLOWTERM, OP_NOT] = new ParseAction(56,TERMOP, EXPR);
            this[FOLLOWTERM, OP_RPAREN] = new ParseAction(90014);
            this[FOLLOWTERM, MULT_COMNT] = new ParseAction(90015);
            this[FOLLOWTERM, COMMENT] = new ParseAction(90016);

            this[TERM, NUMERIC] = new ParseAction(57,FACTOR, FOLLOWFACTOR);
            this[TERM, VAR] = new ParseAction(58,FACTOR, FOLLOWFACTOR);
            this[TERM, APIN] = new ParseAction(59,FACTOR, FOLLOWFACTOR);
            this[TERM, DPIN] = new ParseAction(60,FACTOR, FOLLOWFACTOR);
            this[TERM, STRING] = new ParseAction(61,FACTOR, FOLLOWFACTOR);
            this[TERM, BOOL] = new ParseAction(62,FACTOR, FOLLOWFACTOR);
            this[TERM, OP_MINUS] = new ParseAction(63,FACTOR, FOLLOWFACTOR);
            this[TERM, ARRAYINDEX] = new ParseAction(213,FACTOR, FOLLOWFACTOR);
            this[TERM, OP_LPAREN] = new ParseAction(64,FACTOR, FOLLOWFACTOR);

            this[FOLLOWFACTOR, EOF] = new ParseAction(90017);
            this[FOLLOWFACTOR, WAIT] = new ParseAction(90018);
            this[FOLLOWFACTOR, END] = new ParseAction(90019);
            this[FOLLOWFACTOR, DO] = new ParseAction(90020);
            this[FOLLOWFACTOR, VAR] = new ParseAction(90021);
            this[FOLLOWFACTOR, BEGIN] = new ParseAction(90022);
            this[FOLLOWFACTOR, RETURN] = new ParseAction(90023);
            this[FOLLOWFACTOR, FUNC] = new ParseAction(90024);
            this[FOLLOWFACTOR, CALL] = new ParseAction(90025);
            this[FOLLOWFACTOR, IF] = new ParseAction(90026);
            this[FOLLOWFACTOR, ELSE] = new ParseAction(90027);
            this[FOLLOWFACTOR, APIN] = new ParseAction(90028);
            this[FOLLOWFACTOR, DPIN] = new ParseAction(90029);
            this[FOLLOWFACTOR, OP_EQUAL] = new ParseAction(90030);
            this[FOLLOWFACTOR, OP_OR] = new ParseAction(90031);
            this[FOLLOWFACTOR, OP_NOT] = new ParseAction(90032);
            this[FOLLOWFACTOR, OP_GREATER] = new ParseAction(90033);
            this[FOLLOWFACTOR, OP_LESS] = new ParseAction(90034);
            this[FOLLOWFACTOR, ARRAYINDEX] = new ParseAction(0);
            this[FOLLOWFACTOR, OP_AND] = new ParseAction(90035);
            this[FOLLOWFACTOR, OP_DIVIDE] = new ParseAction(65,FACTOROP, TERM);
            this[FOLLOWFACTOR, OP_MODULO] = new ParseAction(66,FACTOROP, TERM);
            this[FOLLOWFACTOR, OP_TIMES] = new ParseAction(67,FACTOROP, TERM);
            this[FOLLOWFACTOR, OP_PLUS] = new ParseAction(90036);
            this[FOLLOWFACTOR, OP_MINUS] = new ParseAction(90036);
            this[FOLLOWFACTOR, OP_RPAREN] = new ParseAction(90037);
            this[FOLLOWFACTOR, MULT_COMNT] = new ParseAction(90038);
            this[FOLLOWFACTOR, COMMENT] = new ParseAction(90039);

            this[FACTOR, NUMERIC] = new ParseAction(68,VAL);
            this[FACTOR, VAR] = new ParseAction(69,VAL);
            this[FACTOR, APIN] = new ParseAction(70,VAL);
            this[FACTOR, DPIN] = new ParseAction(71,VAL);
            this[FACTOR, STRING] = new ParseAction(72,VAL);
            this[FACTOR, OP_MINUS] = new ParseAction(73,VAL);
            this[FACTOR, BOOL] = new ParseAction(138,VAL);
            this[FACTOR, ARRAYINDEX] = new ParseAction(214,VAL);
            this[FACTOR, OP_LPAREN] = new ParseAction(74,OP_LPAREN, EXPR, OP_RPAREN);

            this[ARRAYACCESSING, EOF] = new ParseAction(0);
            this[ARRAYACCESSING, SEPARATOR] = new ParseAction(0);
            this[ARRAYACCESSING, WAIT] = new ParseAction(0);
            this[ARRAYACCESSING, VAR] = new ParseAction(0);
            this[ARRAYACCESSING, END] = new ParseAction(0);
            this[ARRAYACCESSING, RETURN] = new ParseAction(0);
            this[ARRAYACCESSING, DO] = new ParseAction(0);
            this[ARRAYACCESSING, BEGIN] = new ParseAction(0);
            this[ARRAYACCESSING, FUNC] = new ParseAction(0);
            this[ARRAYACCESSING, CALL] = new ParseAction(0);
            this[ARRAYACCESSING, IF] = new ParseAction(0);
            this[ARRAYACCESSING, ELSE] = new ParseAction(0);
            this[ARRAYACCESSING, OP_MINUS] = new ParseAction(0);
            this[ARRAYACCESSING, OP_OR] = new ParseAction(0);
            this[ARRAYACCESSING, OP_LESS] = new ParseAction(0);
            this[ARRAYACCESSING, OP_GREATER] = new ParseAction(0);
            this[ARRAYACCESSING, OP_AND] = new ParseAction(0);
            this[ARRAYACCESSING, OP_EQUAL] = new ParseAction(0);
            this[ARRAYACCESSING, OP_MODULO] = new ParseAction(0);
            this[ARRAYACCESSING, OP_DIVIDE] = new ParseAction(0);
            this[ARRAYACCESSING, OP_TIMES] = new ParseAction(0);
            this[ARRAYACCESSING, OP_PLUS] = new ParseAction(0);
            this[ARRAYACCESSING, ARRAYINDEX] = new ParseAction(75,ARRAYINDEX, ARRAYACCESSOR);
            this[ARRAYACCESSING, ASSIGN] = new ParseAction(0);
            this[ARRAYACCESSING, OP_RPAREN] = new ParseAction(0);
            this[ARRAYACCESSING, COMMENT] = new ParseAction(0);
            this[ARRAYACCESSING, MULT_COMNT] = new ParseAction(0);
            this[ARRAYACCESSING, APIN] = new ParseAction(0);
            this[ARRAYACCESSING, DPIN] = new ParseAction(0);

            this[ARRAYACCESSOR, NUMERIC] = new ParseAction(205, NUMERIC);
            this[ARRAYACCESSOR, VAR] = new ParseAction(206, VAR);

            this[INDEXER, VAR] = new ParseAction(76,VAR);
            this[INDEXER, NUMERIC] = new ParseAction(77,NUMERIC);

            this[TERMOP, OP_MINUS] = new ParseAction(78,OP_MINUS);
            this[TERMOP, OP_AND] = new ParseAction(79,BOOL_OP);
            this[TERMOP, OP_OR] = new ParseAction(80,BOOL_OP);
            this[TERMOP, OP_GREATER] = new ParseAction(81,BOOL_OP);
            this[TERMOP, OP_LESS] = new ParseAction(82,BOOL_OP);
            this[TERMOP, OP_EQUAL] = new ParseAction(83,BOOL_OP);
            this[TERMOP, OP_NOT] = new ParseAction(84,BOOL_OP);
            this[TERMOP, OP_PLUS] = new ParseAction(85,OP_PLUS);

            this[FACTOROP, OP_MODULO] = new ParseAction(86,OP_MODULO);
            this[FACTOROP, OP_DIVIDE] = new ParseAction(88,OP_DIVIDE);
            this[FACTOROP, OP_TIMES] = new ParseAction(87,OP_TIMES);

            this[MATH_OP, OP_MINUS] = new ParseAction(89,OP_MINUS);
            this[MATH_OP, OP_PLUS] = new ParseAction(90,OP_PLUS);
            this[MATH_OP, OP_DIVIDE] = new ParseAction(91,OP_DIVIDE);
            this[MATH_OP, OP_TIMES] = new ParseAction(92,OP_TIMES);
            this[MATH_OP, OP_MODULO] = new ParseAction(93,OP_MODULO);

            this[BOOL_OP, OP_AND] = new ParseAction(94,OP_AND);
            this[BOOL_OP, OP_OR] = new ParseAction(95,OP_OR);
            this[BOOL_OP, OP_LESS] = new ParseAction(96,OP_LESS, OP_OREQUAL);
            this[BOOL_OP, OP_GREATER] = new ParseAction(97,OP_GREATER, OP_OREQUAL);
            this[BOOL_OP, OP_EQUAL] = new ParseAction(98,OP_EQUAL);
            this[BOOL_OP, OP_NOT] = new ParseAction(99,OP_NOT, OP_EQUAL);

            this[OP_OREQUAL, NUMERIC] = new ParseAction(0);
            this[OP_OREQUAL, VAR] = new ParseAction(0);
            this[OP_OREQUAL, APIN] = new ParseAction(0);
            this[OP_OREQUAL, DPIN] = new ParseAction(0);
            this[OP_OREQUAL, STRING] = new ParseAction(0);
            this[OP_OREQUAL, OP_MINUS] = new ParseAction(0);
            this[OP_OREQUAL, OP_OR] = new ParseAction(100,OP_OR, OP_EQUAL);
            this[OP_OREQUAL, OP_LPAREN] = new ParseAction(0);

            this[VAL, NUMERIC] = new ParseAction(101,NUMERIC);
            this[VAL, VAR] = new ParseAction(102,VAR);
            this[VAL, ARRAYINDEX] = new ParseAction(210,ARRAYACCESSING);
            this[VAL, APIN] = new ParseAction(103,PIN);
            this[VAL, DPIN] = new ParseAction(104,PIN);
            this[VAL, BOOL] = new ParseAction(105,BOOL);
            this[VAL, STRING] = new ParseAction(106,STRING);
            this[VAL, OP_MINUS] = new ParseAction(107,OP_MINUS, NUMERIC);

            this[ARR, EOF] = new ParseAction(0);
            this[ARR, WAIT] = new ParseAction(0);
            this[ARR, END] = new ParseAction(0);
            this[ARR, VAR] = new ParseAction(0);
            this[ARR, BEGIN] = new ParseAction(0);
            this[ARR, RETURN] = new ParseAction(0);
            this[ARR, FUNC] = new ParseAction(0);
            this[ARR, CALL] = new ParseAction(0);
            this[ARR, IF] = new ParseAction(0);
            this[ARR, ELSE] = new ParseAction(0);
            this[ARR, APIN] = new ParseAction(0);
            this[ARR, DPIN] = new ParseAction(0);
            this[ARR, ARRAYLEFT] = new ParseAction(108,ARRAYLEFT, NUMERIC, ARRAYRIGHT);
            this[ARR, COMMENT] = new ParseAction(0);
            this[ARR, MULT_COMNT] = new ParseAction(0);
            this[ARR, ARRAYINDEX] = new ParseAction(0);

            this[PIN, APIN] = new ParseAction(109,APIN);
            this[PIN, DPIN] = new ParseAction(110,DPIN);

            this[IFSTMNT, IF] = new ParseAction(111,IF, EXPR, DO, STMNTS, ELSESTMNT, ENDIF);

            this[ENDIF, END] = new ParseAction(112,END, IF);

            this[ELSESTMNT, END] = new ParseAction(0);
            this[ELSESTMNT, ELSE] = new ParseAction(113,ELSE, ELSEIFSTMNT, STMNTS);

            this[ELSEIFSTMNT, IF] = new ParseAction(114,IF, EXPR, DO, STMNTS, ELSESTMNT);
            this[ELSEIFSTMNT, VAR] = new ParseAction(0);
            this[ELSEIFSTMNT, NUMERIC] = new ParseAction(0);
            this[ELSEIFSTMNT, APIN] = new ParseAction(0);
            this[ELSEIFSTMNT, DPIN] = new ParseAction(0);
            this[ELSEIFSTMNT, OP_MINUS] = new ParseAction(0);
            this[ELSEIFSTMNT, BEGIN] = new ParseAction(0);
            this[ELSEIFSTMNT, RETURN] = new ParseAction(0);
            this[ELSEIFSTMNT, CALL] = new ParseAction(0);
            this[ELSEIFSTMNT, WAIT] = new ParseAction(0);

            this[FUNCCALL, CALL] = new ParseAction(115,CALL, VAR, CALLPARAMS);

            this[FUNCDECL, FUNC] = new ParseAction(116,FUNC, VAR, DECLPARAMS, STMNTS, RETSTMNT, ENDFUNC);

            this[ENDFUNC, END] = new ParseAction(117,END, VAR);

            this[DECLPARAMS, WITH] = new ParseAction(118,WITH, VAR, DECLPARAM);
            this[DECLPARAMS, WAIT] = new ParseAction(0);
            this[DECLPARAMS, END] = new ParseAction(0);
            this[DECLPARAMS, VAR] = new ParseAction(0);
            this[DECLPARAMS, BEGIN] = new ParseAction(0);
            this[DECLPARAMS, RETURN] = new ParseAction(0);
            this[DECLPARAMS, FUNC] = new ParseAction(0);
            this[DECLPARAMS, CALL] = new ParseAction(0);
            this[DECLPARAMS, IF] = new ParseAction(0);
            this[DECLPARAMS, ELSE] = new ParseAction(0);
            this[DECLPARAMS, APIN] = new ParseAction(0);
            this[DECLPARAMS, DPIN] = new ParseAction(0);
            this[DECLPARAMS, COMMENT] = new ParseAction(0);
            this[DECLPARAMS, MULT_COMNT] = new ParseAction(0);

            this[DECLPARAM, SEPARATOR] = new ParseAction(119,SEPARATOR, VAR, DECLPARAM);
            this[DECLPARAM, WAIT] = new ParseAction(0);
            this[DECLPARAM, END] = new ParseAction(0);
            this[DECLPARAM, VAR] = new ParseAction(0);
            this[DECLPARAM, BEGIN] = new ParseAction(0);
            this[DECLPARAM, RETURN] = new ParseAction(0);
            this[DECLPARAM, FUNC] = new ParseAction(0);
            this[DECLPARAM, CALL] = new ParseAction(0);
            this[DECLPARAM, IF] = new ParseAction(0);
            this[DECLPARAM, APIN] = new ParseAction(0);
            this[DECLPARAM, DPIN] = new ParseAction(0);
            this[DECLPARAM, COMMENT] = new ParseAction(0);
            this[DECLPARAM, MULT_COMNT] = new ParseAction(0);


            this[RETSTMNT, RETURN] = new ParseAction(120,RETURN, EXPR);
            this[RETSTMNT, END] = new ParseAction(0);

            this[BEGINSTMNT, BEGIN] = new ParseAction(121,BEGIN, BEGINABLE);

            this[BEGINABLE, FOR] = new ParseAction(122,LOOPF);
            this[LOOPF, FOR] = new ParseAction(123,FOR, VAR, IN, RANGE, DO, STMNTS, ENDFOR);
            this[ENDFOR, END] = new ParseAction(124,END, FOR);

            this[BEGINABLE, WHILE] = new ParseAction(125,LOOPW);
            this[LOOPW, WHILE] = new ParseAction(126,WHILE, EXPR, DO, STMNTS, ENDWHILE);
            this[ENDWHILE, END] = new ParseAction(127,END, WHILE);

            this[RANGE, NUMERIC] = new ParseAction(128,NUMERIC, OP_RANGE, NUMERIC);

            this[WAITSTMNT, WAIT] = new ParseAction(129,WAIT, NUMERIC, TIME_MOD);

            this[TIME_MOD, TIME_HR] = new ParseAction(130,TIME_HR);
            this[TIME_MOD, TIME_MIN] = new ParseAction(131,TIME_MIN);
            this[TIME_MOD, TIME_SEC] = new ParseAction(132,TIME_SEC);
            this[TIME_MOD, TIME_MS] = new ParseAction(133,TIME_MS);

            this[CALLPARAMS, EOF] = new ParseAction(0);
            this[CALLPARAMS, WITH] = new ParseAction(134,WITH, VAL, CALLPARAM);
            this[CALLPARAMS, WAIT] = new ParseAction(0);
            this[CALLPARAMS, END] = new ParseAction(0);
            this[CALLPARAMS, VAR] = new ParseAction(0);
            this[CALLPARAMS, BEGIN] = new ParseAction(0);
            this[CALLPARAMS, RETURN] = new ParseAction(0);
            this[CALLPARAMS, FUNC] = new ParseAction(0);
            this[CALLPARAMS, CALL] = new ParseAction(0);
            this[CALLPARAMS, IF] = new ParseAction(0);
            this[CALLPARAMS, ELSE] = new ParseAction(0);
            this[CALLPARAMS, COMMENT] = new ParseAction(0);
            this[CALLPARAMS, MULT_COMNT] = new ParseAction(0);
            this[CALLPARAMS, OP_RPAREN] = new ParseAction(0);

            this[CALLPARAM, OP_RPAREN] = new ParseAction(0);
            this[CALLPARAM, EOF] = new ParseAction(0);
            this[CALLPARAM, VAR] = new ParseAction(0);
            this[CALLPARAM, WAIT] = new ParseAction(0);
            this[CALLPARAM, END] = new ParseAction(0);
            this[CALLPARAM, BEGIN] = new ParseAction(0);
            this[CALLPARAM, RETURN] = new ParseAction(0);
            this[CALLPARAM, FUNC] = new ParseAction(0);
            this[CALLPARAM, CALL] = new ParseAction(0);
            this[CALLPARAM, IF] = new ParseAction(0);
            this[CALLPARAM, ELSE] = new ParseAction(0);
            this[CALLPARAM, COMMENT] = new ParseAction(0);
            this[CALLPARAM, MULT_COMNT] = new ParseAction(0);
            this[CALLPARAM, SEPARATOR] = new ParseAction(135,SEPARATOR, VAL, CALLPARAM);
            
            this[PROG, ARRAYINDEX] = new ParseAction(210,STMNTS);
        }
    }
}