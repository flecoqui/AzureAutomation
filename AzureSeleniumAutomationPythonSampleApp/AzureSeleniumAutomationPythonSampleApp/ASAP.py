import time
import argparse
import sys
import os
import platform
from selenium import webdriver
from selenium.webdriver.common.keys import Keys

path= 'C:/temp/Chrome/chromedriver.exe'
login = "login"
password = "password"
parser = argparse.ArgumentParser(description='Azure Selenium Automation in Python (ASAP).')
parser.add_argument('--path', help='Path where Chromedriver is stored' )
parser.add_argument('--login', help='Login for the connnection with Azure Portal' )
parser.add_argument('--password', help='Login for the connnection with Azure Portal' )
args = parser.parse_args()

if args.login == None:
   parser.print_help();
   sys.exit(0);
if args.password == None:
   parser.print_help();
   sys.exit(0);
try:
    if args.path == None:
        localpath = os.getcwd()
        localpath = localpath.replace('\\','/')
        if platform.system() == 'Windows':
            args.path = localpath + '/chromedriver.exe' 
        else:
            args.path = localpath + '/chromedriver' 
    driver = webdriver.Chrome(executable_path=args.path)
except Exception as inst: 
    print('Error while launching Chrome:' , inst)
else:
    try:
        driver.get("https://ea.azure.com")
        elem = driver.find_element_by_id("i0116")
        elem.clear()
        elem.send_keys(args.login)
        elem.send_keys(Keys.RETURN)
        time.sleep(1)
        elem = driver.find_element_by_id("i0118")
        elem.clear()
        elem.send_keys(args.password)
        elem.send_keys(Keys.RETURN)
        time.sleep(1)
        elem = driver.find_element_by_id("idSIButton9")
        elem.click()
        driver.close()
    except Exception as inst: 
        print('Error while opening the session:' , inst)

