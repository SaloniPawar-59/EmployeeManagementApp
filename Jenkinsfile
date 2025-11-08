pipeline {
    agent any

    environment {
        AWS_REGION = "ap-south-1"
        S3_BUCKET = "elasticbeanstalk-ap-south-1-304686171763"
        APP_NAME = "EmployeeManagementApp"
        EB_ENV = "EmployeeManagementApp-dev"
    }

    options {
        timeout(time: 15, unit: 'MINUTES')
        disableConcurrentBuilds()
        buildDiscarder(logRotator(numToKeepStr: '5'))
    }

    stages {
        stage('Verify .NET SDK Version') {
            steps {
                sh 'dotnet --version'
            }
        }

        stage('Checkout Code') {
            steps {
                git branch: 'main', url: 'https://github.com/SaloniPawar-59/EmployeeManagementApp.git'
            }
        }

        stage('Clean, Restore & Build') {
            steps {
                sh '''
                echo "🧹 Cleaning project..."
                dotnet clean EmployeeManagementApp/EmployeeManagementApp.csproj
                echo "📦 Restoring dependencies..."
                dotnet restore EmployeeManagementApp/EmployeeManagementApp.csproj --verbosity minimal
                echo "⚙️ Building project..."
                dotnet build EmployeeManagementApp/EmployeeManagementApp.csproj -c Release --no-restore --verbosity minimal
                '''
            }
        }

        stage('Publish Artifact') {
            steps {
                sh 'dotnet publish EmployeeManagementApp/EmployeeManagementApp.csproj -c Release -o output'
            }
        }

        stage('Zip Artifact') {
            steps {
                sh 'cd output && zip -r ../EmployeeManagementApp.zip .'
            }
        }

        stage('Upload & Deploy') {
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    sh '''
                    aws s3 cp EmployeeManagementApp.zip s3://$S3_BUCKET/

                    aws elasticbeanstalk create-application-version \
                        --application-name $APP_NAME \
                        --version-label "build-$BUILD_ID" \
                        --source-bundle S3Bucket=$S3_BUCKET,S3Key=EmployeeManagementApp.zip

                    aws elasticbeanstalk update-environment \
                        --environment-name $EB_ENV \
                        --version-label "build-$BUILD_ID"
                    '''
                }
            }
        }
    }

    post {
        success {
            echo "✅ Deployment Successful!"
        }
        failure {
            echo "❌ Deployment Failed!"
        }
    }
}
