import os

template = "<script>\n\timport $ from \"jquery\"; \n\timport APIHelper from \"../mixins/APIHelper.js\";\n    \n\texport default{\n\t\tmixins:[APIHelper],\n\t\tdata(){\n\t\t\treturn{\n\t\t\t\t//Varible area\n\t\t\t}\n\t\t},\n\t\tmethods:{\n\t\t\t//Methods area\n\t\t},\n\t\tmounted(){\n\t\t\t//Init component method\n\t\t}\n\t}\n</script>\n\n<template>\n\t<body>\n\t\t<!--put html here-->\n\t</body>\n</template>\n\n<style scoped>\n\n</style>\n"

while True:
    f_name = input("Введите название компонента: ")
    f_name = f_name[0].upper() + f_name[1:]

    with open(f'{f_name}.vue','w') as file:
        file.write(template)


    cwd = os.getcwd()
    path = '..'+cwd[cwd.find("\\src\\")+4:].replace("\\","/")+f'/{f_name}.vue'
    print("import "+f_name+" from '" +path+"'")
