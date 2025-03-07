pipeline {
    agent any

    environment {
        // Variables de entorno (puedes configurarlas en Jenkins)
        NODE_ENV = 'production'
        PORT = 3000 // Puerto para la aplicación (ajusta según tu proyecto)
    }

    stages {
        stage('Clonar repositorio') {
            steps {
                git branch: 'main', url: 'https://github.com/AnthonyDiazCabanillas/hospital.git'
            }
        }

        stage('Instalar dependencias') {
            steps {
                sh 'npm install'
            }
        }

        stage('Ejecutar pruebas') {
            steps {
                sh 'npm test' // Asegúrate de que el proyecto tenga scripts de prueba en package.json
            }
        }

        stage('Construir proyecto') {
            steps {
                sh 'npm run build' // Asegúrate de que el proyecto tenga un script de construcción en package.json
            }
        }

        stage('Desplegar') {
            steps {
                // Ejemplo de despliegue en un servidor remoto usando SSH
                sh '''
                    ssh usuario@servidor-remoto '
                        cd /ruta/al/proyecto &&
                        git pull origin main &&
                        npm install --production &&
                        pm2 restart hospital-app
                    '
                '''
                // Nota: Ajusta el comando SSH según tu entorno de despliegue.
            }
        }
    }

    post {
        success {
            echo '¡Pipeline ejecutado con éxito!'
        }
        failure {
            echo 'Pipeline falló. Revisa los logs para más detalles.'
        }
    }
}