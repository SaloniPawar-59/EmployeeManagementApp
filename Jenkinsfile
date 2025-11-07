pipeline {
    agent any

    environment {
        AWS_REGION = "ap-south-1"
        S3_BUCKET = "elasticbeanstalk-ap-south-1-304686171763"  
        APP_NAME = "EmployeeManagementApp"
        EB_ENV = "EmployeeManagementApp-dev"
    }

    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', url: 'https://github.com/<your-username>/EmployeeManagementApp.git'
            }
        }

        stage('Restore & Build') {
            steps {
                sh 'dotnet restore EmployeeManagementApp/EmployeeManagementApp.csproj'
                sh 'dotnet build EmployeeManagementApp/EmployeeManagementApp.csproj -c Release'
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

        stage('Upload to S3') {
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    sh 'aws s3 cp EmployeeManagementApp.zip s3://$S3_BUCKET/'
                }
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    sh '''
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
