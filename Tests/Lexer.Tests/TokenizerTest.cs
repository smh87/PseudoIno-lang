using System.Linq;
using System.Collections.Generic;
using Lexer.Objects;
using Lexer.Exceptions;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Lexer.Tests
{
    [TestFixture]
    public class TokenizerTest
    {
        #region Dummy accept strings
        private const string dummy_1 = @"# This is a dummy program to test the token generator
            #< This multiline comment
            should also be accepted >#
            a is 4
            b is 6
            c is a + b
            func test with x, y, z
                x + y equals z
            end test";
        private const string dummy_2 = @"
        
        
        
        a is 2";
        private const string dummy_3 = @"";
        private const string dummy_4 = @"func foo with (numeric bar, string baz)
            x is bar
            y is baz
        end foo
        
        a is 4
        b is ""human""
        c is ""dog""
        if b equals c do
            a is a + 1
        end if";

        private const string dummy_5 = @"func foo
            x is bar
            y is baz
        end foo
        
        a is 4.
        b is ""human""
        c is ""dog""
        if b equals c do
            a is a + 1
        end if";

        

        private const string dummy_6 = @"func foo
            x is bar
            y is baz
        end foo
        
        a <- 4.
        b := ""human""
        c is ""dog""
        if b == c do
            a is a + 1
        else if b <= c do
            a is a - 1
        else if b >= c do
            a = 1
        end if";

        #endregion

        #region Dummy reject strings

        private const string reject_dummy_1 = @"# This is a dummy program to test the token generator
            #< This multiline comment
            should not be accepted
            a is 4
            b is 6.2
            c is a + b
            func test with (numeric x, numeric y, numeric z)
                x + y equals z?
            end test";
        private const string reject_dummy_2 = @"
        
        
        
        a is 2...5";
        private const string reject_dummy_3 = @"func foo with (numeric bar, string baz)
            x is bar
            y is baz
        end foo
        
        a is 4
        b is ""human
        c is ""dog""
        if b equals c?
            a is a + 1
        end if";

        private const string reject_dummy_4 = @"
        a is 3 | 2
        b is 3 & 2
        c is 2 || 1
        d is 4 && 3
        e is 4 < 1
        f[a]a@";

        #endregion

        [TestCase(dummy_1, 28)]
        [TestCase(dummy_2, 3)]
        [TestCase(dummy_3, 0)]
        [TestCase(dummy_4, 39)]
        [TestCase(dummy_5, 31)]
        [TestCase(dummy_6, 55)]
        public void Test_GenerateTokens_CanTraverseEntireFileWithNoErrorsAndCorrentAmountOfTokens(string content, int expectedAmountOfTokens)
        {

            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();
            
            Assert.AreEqual(expectedAmountOfTokens, tokenizer.Tokens.Count, "Tokenizer did not generate the correct amount of tokens.");
        }

        [TestCase(reject_dummy_1)]
        [TestCase(reject_dummy_2)]
        [TestCase(reject_dummy_3)]
        [TestCase(reject_dummy_4)]
        public void Test_GenerateTokens_CanTraverseEntireFileAndThrowErrors(string content)
        {
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();
            Assert.IsTrue(Tokenizer.HasError);
        }

        [Test]
        public void Test_GenerateTokens_TokenListIsEmpty()
        {
            string content = "";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(0, tokenizer.Tokens.Count, "TokenList had the wrong amount of tokens");
        }

        [Test]
        public void Test_GenerateTokens_TokenListNotEmpty()
        {
            string content = "a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(3, tokenizer.Tokens.Count, "TokenList generated the wrong amound of tokens. Should contain elements more than just PROG and EOF");
        }

        [Test]
        public void Test_GenerateTokens_CanScanComments()
        {
            string content = "# This is a comment in the language";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(1,tokenizer.Tokens.Count, "The tokenizer did not recognise a comment");
        }

        [Test]
        public void Test_GenerateTokens_GeneratesCommentToken()
        {
            string content = "# comment";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(TokenType.COMMENT, tokenizer.Tokens.First.Value.Type, "The tokenizer did not recognise the comment as a comment");
        }


        [Test]
        public void Test_GenerateTokens_CanScanMultilineComments()
        {
            string content = "#< comment >#";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(1, tokenizer.Tokens.Count, "The tokenizer did not recognise a comment");

        }
        [Test]
        public void Test_GenerateTokens_GeneratesMultiLineCommentToken()
        {
            string content = "#< comment >#";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();

            Assert.AreEqual(TokenType.MULT_COMNT, tokenizer.Tokens.First.Value.Type, "The tokenizer did not recognise the comment as a comment");
        }

        [Test]
        public void Test_GenerateTokens_ThrowsExceptionOnInvalidMultilineComment()
        {
            string content = "#< comment";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();
            Assert.IsTrue(Tokenizer.HasError);
        }

        [Test]
        public void Test_GenerateTokens_CanGenerateRanges()
        {
            string content = "a is 0..5";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.GenerateTokens();


            Assert.AreEqual(5, tokenizer.Tokens.Count, $"The tokenizer found the wrong amount of tokens.");
        }

        [Test]
        public void Test_GenerateTokens_CanNotGenrateInvalidRanges()
        {
            string content = "a is 0...5";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);

            tokenizer.GenerateTokens();
            Assert.IsTrue(Tokenizer.HasError);
        }

        [Test]
        public void Test_Pop_ReturnsCorrectCharacter()
        {
            string content = @"a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);

            Assert.AreEqual('a', tokenizer.Pop(), "Pop did not set the correct character");
        }

        [Test]
        public void Test_Pop_ShouldNotReturnWrongCharacter()
        {
            string content = @"a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);

            Assert.AreNotEqual('W', tokenizer.Pop(), "Pop got the correct character when it should not");
        }

        [Test]
        public void Test_Peek_ShouldPeekCurrectChar()
        {
            string content = @"a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.Pop();

            Assert.AreEqual(' ', tokenizer.Peek(), "Peek did not get the correct character");
        }

        [Test]
        public void Test_Peek_ShouldNotPeekWrongChar()
        {
            string content = @"a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.Pop();

            Assert.AreNotEqual('W', tokenizer.Peek(), "Peek got the correct character when it should not");
        }

        [Test]
        public void Test_Peek_CanPeekMultipleCharacters()
        {
            string content = @"a is 4";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.Pop();

            Assert.AreEqual(' ', tokenizer.Peek(), "Tokenizer did not find the correct character");
            Assert.AreEqual('i', tokenizer.Peek(2), "Tokenizer did not find the correct character");
            Assert.AreEqual(' ', tokenizer.Peek(), "Tokenizer did not find the correct character");
        }

        [Test]
        public void Test_Pop_ReturnProvidesCorrectLineNumber()
        {
            string content = "a is 4\nb is 5\nc is a + b";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            while (tokenizer.Pop() != '\n')
            {/* Intentional blank */}

            tokenizer.Pop();

            Assert.AreEqual(2, tokenizer.Line, "Tokenizer did not return correct line number.");
        }

        [Test]
        public void Test_Peek_CanPeekAhead()
        {
            string content = "abcdefghijklmnopqrstuvxyz";
            StreamReader FakeReader = CreateFakeReader(content, Encoding.UTF8);

            Tokenizer tokenizer = new Tokenizer(FakeReader);
            tokenizer.Pop();
            char lookAheadChar = tokenizer.Peek(2);
            tokenizer.Pop();

            Assert.AreEqual(lookAheadChar, tokenizer.Peek(), "Tokenizer did not peek ahead correctly.");
        }


        // Helper functions

        public StreamReader CreateFakeReader(string content, Encoding enc)
        {
            byte[] fakeBytes = enc.GetBytes(content);
            return new StreamReader(new MemoryStream(fakeBytes), enc, false);
        }
    }
}