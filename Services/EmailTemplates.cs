using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpireOneRestAPIFHS.Services
{
    public class EmailTemplates
    {
        public static string Build_1M_WinnerEmail()
        {
            string emailTemplate = @"
                <html>
                <head>
                    <style>
                        body {{ 
                            font-family: 'Arial', sans-serif;
                            background-color: #f4f4f4;
                            margin: 0; 
                            padding: 20px 0; 
                        }}
                        .container {{ 
                            max-width: 600px; 
                            margin: 20px auto;
                            background-color: #ffffff;
                            padding: 0;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            border-top: 4px solid #2E7D32;
                            overflow: hidden;
                        }}
                        .header {{ 
                            background-color: #2E7D32;
                            color: #ffffff;
                            padding: 25px;
                            text-align: center;
                        }}
                        .header h2 {{
                            margin: 0;
                            font-size: 28px;
                            font-weight: bold;
                        }}
                        .winner-badge {{
                            display: inline-block;
                            font-size: 24px;
                            margin: 15px 0;
                            color: #FFD700;
                            font-weight: bold;
                        }}
                        .content {{ 
                            padding: 25px;
                        }}
                        .content p {{ 
                            line-height: 1.6;
                            color: #333333;
                            font-size: 15px;
                        }}
                        .content strong {{ 
                            color: #1a1a1a;
                            font-weight: bold;
                        }}
                        .winner-notice {{
                            background-color: #E8F5E9;
                            padding: 15px;
                            border-radius: 6px;
                            margin: 15px 0;
                            border-left: 4px solid #2E7D32;
                        }}
                        .winner-notice h3 {{
                            color: #2E7D32;
                            font-size: 18px;
                            margin-top: 0;
                        }}
                        .numbers {{ 
                            background-color: #f5f5f5;
                            padding: 15px;
                            border-radius: 6px;
                            margin: 15px 0;
                            border-left: 4px solid #0073e6;
                        }}
                        .numbers p {{
                            margin: 10px 0;
                            font-size: 15px;
                        }}
                        .match-count {{ 
                            font-size: 16px;
                            color: #2E7D32;
                            font-weight: bold;
                            background-color: #E8F5E9;
                            padding: 12px;
                            border-radius: 6px;
                            margin: 15px 0;
                            text-align: center;
                        }}
                        .numbers-highlight {{
                            color: #2E7D32;
                            font-weight: bold;
                            font-size: 16px;
                        }}
                        .action-section {{
                            background-color: #f9f9f9;
                            padding: 15px;
                            border-radius: 6px;
                            margin: 15px 0;
                        }}
                        .action-section h3 {{
                            color: #2E7D32;
                            font-size: 16px;
                            margin-top: 0;
                        }}
                        .action-section ol {{
                            margin: 10px 0;
                            padding-left: 20px;
                            color: #333333;
                        }}
                        .action-section li {{
                            margin: 8px 0;
                            line-height: 1.5;
                        }}
                        .disclaimer {{
                            background-color: #FFF3E0;
                            padding: 12px;
                            border-radius: 6px;
                            margin: 15px 0;
                            border-left: 4px solid #F57C00;
                            font-size: 14px;
                            color: #555555;
                        }}
                        .footer {{ 
                            text-align: center; 
                            margin-top: 20px;
                            font-size: 12px; 
                            color: #888888;
                            padding-top: 15px;
                            border-top: 1px solid #e0e0e0;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>LottoJump Winner Notification</h2>
                            <div class='winner-badge'>🎉 Big Winner! 🎉</div>
                        </div>
            
                        <div class='content'>
                            <div class='winner-notice'>
                                <h3>Congratulations!</h3>
                                <p>You have won on your lottery ticket. 5 of your numbers have matched the winning draw.</p>
                            </div>
                
                            <p><strong>Notification Date:</strong> {12}</p>
                            <p>Hello {1},</p>
                
                            <p>We are pleased to inform you that your lottery ticket has matched the winning numbers drawn on {0}.</p>
                
                            <p><strong>Your Account Information:</strong></p>
                            <p>User ID: {2}<br>
                            Card Name: {3}</p>
                
                            <div class='numbers'>
                                <p><strong>Lottery Game:</strong> {13}</p>
                                <p><strong>Your Numbers:</strong><br>
                                <span class='numbers-highlight'>{4}</span></p>
                                <p><strong>Your Bonus Number:</strong> <span class='numbers-highlight'>{5}</span></p>
                                <p><strong>Winning Numbers:</strong><br>
                                <span class='numbers-highlight'>{6}</span></p>
                                <p><strong>Bonus Number:</strong> <span class='numbers-highlight'>{7}</span></p>
                            </div>
                
                            <div class='match-count'>
                                Total Matches: {9} - {10}
                            </div>
                
                            <div class='action-section'>
                                <h3>How to Claim Your Prize</h3>
                                <ol>
                                    <li>Sign the back of your winning lottery ticket immediately</li>
                                    <li>Locate your nearest official lottery office or retailer</li>
                                    <li>Bring your signed ticket and valid photo identification</li>
                                    <li>Present your documents and claim your winnings</li>
                                </ol>
                            </div>
                
                            <div class='disclaimer'>
                                <strong>Important Notice:</strong> Please verify this notification with your account on {11} before claiming any prize. Official lottery offices will verify all winning tickets. We recommend consulting with a tax professional or financial advisor before claiming your prize.
                            </div>
                
                            <p>If you have any questions about your win or need assistance, please visit your {11} account or contact our support team.</p>
                
                            <p>Thank you for playing with LottoJump!</p>
                        </div>
            
                        <div class='footer'>
                            <p>&copy; {8} {11}. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return emailTemplate;
        }

        /// <summary>
        /// Jackpot-style winner email — flashy/celebratory style.
        /// Uses the same placeholder map.
        /// </summary>
        public static string Build_JP_WinnerEmail()
        {
            string emailTemplate = @"
                <html>
                <head>
                    <style>
                        body {{ 
                            font-family: 'Arial', sans-serif;
                            background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
                            margin: 0; 
                            padding: 20px 0; 
                        }}
                        .container {{ 
                            max-width: 600px; 
                            margin: 20px auto;
                            background: linear-gradient(135deg, #ffffff 0%, #f0f0f0 100%);
                            padding: 0;
                            border-radius: 12px;
                            box-shadow: 0 0 40px rgba(255, 215, 0, 0.4), 0 0 20px rgba(0, 0, 0, 0.3);
                            border: 3px solid #FFD700;
                            overflow: hidden;
                        }}
                        .header {{ 
                            background: linear-gradient(135deg, #FFD700 0%, #FFA500 100%);
                            color: #000000;
                            padding: 30px;
                            text-align: center;
                            position: relative;
                            overflow: hidden;
                        }}
                        .header h2 {{
                            margin: 0;
                            font-size: 32px;
                            font-weight: bold;
                            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
                        }}
                        .jackpot-banner {{
                            display: inline-block;
                            animation: pulse 1.5s ease-in-out infinite;
                            font-size: 34px;
                            font-weight: bold;
                            margin: 15px 0;
                            color: #FF0000;
                            text-shadow: 3px 3px 0px #FFD700, 6px 6px 0px rgba(0,0,0,0.2);
                            letter-spacing: 3px;
                        }}
                        @keyframes pulse {{
                            0%, 100% {{ transform: scale(1); }}
                            50% {{ transform: scale(1.1); }}
                        }}
                        @keyframes spin {{
                            0% {{ transform: rotate(0deg); }}
                            100% {{ transform: rotate(360deg); }}
                        }}
                        .confetti {{
                            position: absolute;
                            width: 10px;
                            height: 10px;
                            animation: confetti-fall 3s ease-out forwards;
                        }}
                        @keyframes confetti-fall {{
                            to {{
                                transform: translateY(400px) rotate(720deg);
                                opacity: 0;
                            }}
                        }}
                        .star {{
                            display: inline-block;
                            animation: spin 2s linear infinite;
                            margin: 0 10px;
                            font-size: 24px;
                        }}
                        .content {{ 
                            padding: 30px;
                        }}
                        .content p {{ 
                            line-height: 1.8;
                            color: #333333;
                            font-size: 15px;
                        }}
                        .content strong {{ 
                            color: #000000;
                            font-weight: bold;
                        }}
                        .winner-highlight {{
                            background: linear-gradient(135deg, #FFD700 0%, #FFA500 100%);
                            padding: 20px;
                            border-radius: 8px;
                            margin: 20px 0;
                            text-align: center;
                            border: 2px solid #FF0000;
                            box-shadow: 0 0 20px rgba(255, 215, 0, 0.5);
                        }}
                        .winner-highlight h3 {{
                            color: #FF0000;
                            font-size: 24px;
                            margin: 0;
                            text-transform: uppercase;
                            letter-spacing: 2px;
                            animation: pulse 1.5s ease-in-out infinite;
                        }}
                        .numbers {{ 
                            background: linear-gradient(135deg, #f0f8ff 0%, #e6f3ff 100%);
                            padding: 20px;
                            border-radius: 8px;
                            margin: 20px 0;
                            border-left: 5px solid #FFD700;
                            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                        }}
                        .numbers p {{
                            margin: 12px 0;
                            font-size: 16px;
                        }}
                        .match-count {{ 
                            font-size: 22px;
                            color: #FF0000;
                            font-weight: bold;
                            background: #FFFACD;
                            padding: 15px;
                            border-radius: 8px;
                            text-align: center;
                            box-shadow: 0 0 15px rgba(255, 215, 0, 0.4);
                            animation: pulse 1.5s ease-in-out infinite;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background: linear-gradient(135deg, #FF0000 0%, #CC0000 100%);
                            color: #FFFFFF;
                            padding: 15px 30px;
                            margin: 20px auto;
                            border-radius: 8px;
                            text-align: center;
                            font-weight: bold;
                            font-size: 16px;
                            text-decoration: none;
                            box-shadow: 0 0 20px rgba(255, 0, 0, 0.4);
                            display: block;
                            text-align: center;
                            animation: pulse 1.5s ease-in-out infinite;
                        }}
                        .footer {{ 
                            text-align: center; 
                            margin-top: 30px;
                            font-size: 12px; 
                            color: #666666;
                            padding-top: 20px;
                            border-top: 2px solid #FFD700;
                        }}
                        .celebration-text {{
                            font-size: 28px;
                            text-align: center;
                            margin: 20px 0;
                            animation: pulse 1.5s ease-in-out infinite;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h3>🎰 CONGRATULATIONS! 🎰</h3>
                            <div class='jackpot-banner'>
                                🏆 <br> JACKPOT WINNER! <br> 🏆
                            </div>
                            <div class='star'>⭐</div>
                            <div class='star'>⭐</div>
                            <div class='star'>⭐</div>
                        </div>
            
                        <div class='content'>
                            <div class='winner-highlight'>
                                <h4>🎉 YOU'VE WON BIG! 🎉</h4>
                                <p style='font-size: 14px; margin: 10px 0;'>All numbers matched!</p>
                            </div>
                
                            <p><strong>Date:</strong> {12}</p>
                            <p>Hello <strong>{1}</strong>,</p>
                
                            <p>🥳 <strong>UNBELIEVABLE NEWS!</strong> 🥳</p>
                            <p>You have matched <strong>ALL numbers</strong> on your Lotto Ticket! This is a <strong>LIFE-CHANGING</strong> moment!</p>
                
                            <p><strong>Card Name:</strong> {3}</p>
                
                            <div class='numbers'>
                                <p><strong>🎮 Lotto Game:</strong> {13}</p>
                                <p><strong>🎫 Your Winning Numbers:</strong> <br><span style='font-size: 18px; color: #FF0000; font-weight: bold;'>{4}</span></p>
                                <p><strong>🎲 Your Bonus Number:</strong> <span style='font-size: 18px; color: #FF0000; font-weight: bold;'>{5}</span></p>
                                <hr style='border: 1px solid #FFD700;'>
                                <p><strong>✅ Winning Numbers:</strong> <br><span style='font-size: 18px; color: #228B22; font-weight: bold;'>{6}</span></p>
                                <p><strong>✅ Bonus Number:</strong> <span style='font-size: 18px; color: #228B22; font-weight: bold;'>{7}</span></p>
                                <p><strong>📅 Draw Date:</strong> {0}</p>
                            </div>
                
                            <div class='match-count'>
                                ⭐ Perfect Match! All {9} Numbers + Bonus! ⭐<br>
                                <strong>JACKPOT: {10}</strong>
                            </div>
                
                            <p style='text-align: center; font-size: 18px; color: #FF0000; font-weight: bold;'>
                                🎊 YOU ARE A MILLIONAIRE! 🎊
                            </p>
                
                            <div class='celebration-text'>
                                🍾🎉🎁🎉🍾
                            </div>
                
                            <div class='cta-button'>
                                📍 CLAIM YOUR PRIZE NOW! 📍
                            </div>
                
                            <p><strong>NEXT STEPS:</strong></p>
                            <ol style='line-height: 1.8; color: #333333;'>
                                <li>Sign the back of your winning ticket</li>
                                <li>Go to your nearest lottery office</li>
                                <li>Bring valid ID and social security number</li>
                                <li>Claim your JACKPOT prize!</li>
                            </ol>
                
                            <p style='background: #FFF8DC; padding: 15px; border-radius: 8px; border-left: 4px solid #FFD700;'>
                                <strong>⚠️ IMPORTANT:</strong> Lottery offices verify all winners. Please bring your ticket and valid identification. We recommend consulting with a financial advisor before claiming.
                            </p>
                
                            <p style='text-align: center; font-size: 16px; font-weight: bold; color: #FF0000;'>
                                CONGRATULATIONS AGAIN! YOUR LIFE JUST CHANGED FOREVER! 🎰
                            </p>
                        </div>
            
                        <div class='footer'>
                            <p>&copy; {8} {11}. All rights reserved.</p>
                            <p>Thank you for playing responsibly!</p>
                        </div>
                    </div>
                </body>
                </html>";

            return emailTemplate;
        }

        /// <summary>
        /// General matches email (Powerball/Mega/etc). Uses BonusBallMatchText at {14}.
        /// </summary>
        public static string BuildMatchEmail()
        {
            string emailTemplate = @"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif;
                            background-color: #f4f4f4; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 20px auto;
                            background-color: #ffffff; padding: 20px; border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                            .header {{ background-color: #007bff; color: #ffffff;
                            padding: 10px; text-align: center; border-radius: 8px 8px 0 0; }}
                            .content {{ padding: 20px; }}
                            .content p {{ line-height: 1.6; color: #333333; }}
                            .content strong {{ color: #000000; }}
                            .numbers {{ background-color: #f8f9fa; padding: 10px;
                            border-radius: 5px; margin: 10px 0; }}
                            .match-count {{ font-size: 18px; color: #007bff; font-weight: bold; }}
                            .footer {{ text-align: center; margin-top: 20px;
                            font-size: 12px; color: #888888; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>LottoJump Update Notification</h2>
                            </div>
                            <div class='content'>
                                <p><strong>Date:</strong> {12}</p>
                                <p>Hello {1},</p>
                                <p>UserID: {2}</p>
                                <p>This email is to inform you about recent matches to your Lotto Tickets.</p>
                                <p><strong>Card Name:</strong> {3}</p>
        
                                <div class='numbers'>
                                    <p><strong>Lotto Game:</strong> {13}</p>
                                    <p><strong>Your Numbers:</strong> {4} <br> <strong>Your Bonus:</strong> {5}</p>
                                    <p><strong>Winning Numbers:</strong> {6} <br> <strong>Winning Bonus:</strong> {7}</p>
                                    <p><strong>Bonus Ball Match:</strong> {8}</p>
                                    <p><strong>Draw Date:</strong> {0}</p>
                                </div>
        
                                <p class='match-count'>Matches ({9}): <strong>{10}</strong></p>
        
                                <p>Please verify against your account at {11} and follow official claiming procedures for your jurisdiction.</p>
                                <p>Congrats!</p>
                                <p>{11}</p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {8} {11}. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

            return emailTemplate;

        }

        /// <summary>
        /// Cash 5 (no bonus ball) match email — indices aligned with the shared map.
        /// </summary>
        public static string Build_C5_MatchEmail()
        {
            string emailTemplate = @"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif;
                            background-color: #f4f4f4; margin: 0; padding: 0; }}
                            .container {{ max-width: 600px; margin: 20px auto;
                            background-color: #ffffff; padding: 20px; border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                            .header {{ background-color: #B3FF00; color: #000000;
                            padding: 10px; text-align: center; border-radius: 8px 8px 0 0; }}
                            .content {{ padding: 20px; }}
                            .content p {{ line-height: 1.6; color: #333333; }}
                            .content strong {{ color: #000000; }}
                            .numbers {{ background-color: #f8f9fa; padding: 10px;
                            border-radius: 5px; margin: 10px 0; }}
                            .match-count {{ font-size: 18px; color: #2E7D32; font-weight: bold; }}
                            .footer {{ text-align: center; margin-top: 20px;
                            font-size: 12px; color: #888888; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>LottoJump Update Notification</h2>
                            </div>
                            <div class='content'>
                                <p><strong>Date:</strong> {12}</p>
                                <p>Hello {1},</p>
                                <p>UserID: {2}</p>
                                <p>This email is to inform you about recent matches to your Lotto Tickets.</p>
                                <p><strong>Card Name:</strong> {3}</p>
                                <div class='numbers'>
                                    <p><strong>Lotto Game:</strong> {13}</p>
                                    <p><strong>Your Numbers:</strong> {4}</p>
                                    <p><strong>Winning Numbers:</strong> {6}</p>
                                    <p><strong>Draw Date:</strong> {0}</p>
                                </div>
                                <p class='match-count'>Matches ({9}): <strong>{10}</strong></p>
                                <p>Please verify against your account at {11} and follow official claiming procedures.</p>
                                <p>Congrats!</p>
                                <p>{11}</p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {8} {11}. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

            return emailTemplate;
        }

    }
}